using System.Security.Cryptography;
using System.Text;
using Hangfire;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Survey_Basket.Contracts.Authentication;
using Survey_Basket.Helpers;

namespace Survey_Basket.Authentication;

public class AuthService(
    UserManager<ApplicationUser> userManager,
    IJWTProvider jwtProvider ,
    SignInManager<ApplicationUser> signInManager,
    ILogger<AuthService> logger,
    IEmailSender emailSender,
    IHttpContextAccessor httpContextAccessor
                        ) : IAuthService
{
    private readonly UserManager<ApplicationUser> userManager = userManager;
    private readonly IJWTProvider jwtProvider = jwtProvider;
    private readonly SignInManager<ApplicationUser> signInManager = signInManager;
    private readonly ILogger<AuthService> logger = logger;
    private readonly IEmailSender emailSender = emailSender;
    private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;
    private readonly int refreshTokenExpiryDays = 14;

    public async Task<OneOf<AuthResponse , Error>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
            return UserErrors.InvalidEmailOrPassword;
        var result = await signInManager.PasswordSignInAsync(user, password, false, false);
        if (result.Succeeded)
        {
            var (token, expiresIn) = jwtProvider.GenerateToken(user);

            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpiryDate = DateTime.UtcNow.AddDays(refreshTokenExpiryDays);

            user.refreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                ExpiresAt = refreshTokenExpiryDate,
            });

            await userManager.UpdateAsync(user);

            return new AuthResponse(user.Id, user.FirstName, user.LastName, user.Email!, token, expiresIn, refreshToken, refreshTokenExpiryDate);
        }

        return result.IsNotAllowed
            ? UserErrors.EmailNotConfirmed
            : UserErrors.InvalidEmailOrPassword;


    }

    public async Task<AuthResponse?> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = jwtProvider.ValidateToken(token);
        if (userId is null)
            return null;
        var user = userManager.Users.FirstOrDefault(x => x.Id == userId);
        if (user is null)
            return null;
        var userRefreshToken = user.refreshTokens.FirstOrDefault(x => x.Token == refreshToken && x.IsActive);
        if (userRefreshToken is null)
            return null;
        userRefreshToken.RevokedOn = DateTime.UtcNow;

        var (newToken, expiresIn) = jwtProvider.GenerateToken(user);

        var newRefreshToken = GenerateRefreshToken();
        var refreshTokenExpiryDate = DateTime.UtcNow.AddDays(refreshTokenExpiryDays);

        user.refreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            ExpiresAt = refreshTokenExpiryDate,
        });

        await userManager.UpdateAsync(user);

        return new AuthResponse(user.Id, user.FirstName, user.LastName, user.Email!, newToken, expiresIn, newRefreshToken, refreshTokenExpiryDate);
    }

    public async Task<bool> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = jwtProvider.ValidateToken(token);
        if (userId is null)
            return false;
        var user = userManager.Users.FirstOrDefault(x => x.Id == userId);
        if (user is null)
            return false;
        var userRefreshToken = user.refreshTokens.FirstOrDefault(x => x.Token == refreshToken && x.IsActive);
        if (userRefreshToken is null)
            return false;
        userRefreshToken.RevokedOn = DateTime.UtcNow;

        await userManager.UpdateAsync(user);
        return true;
    }


    public async Task<Error?> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var isEmailExists = await userManager.FindByEmailAsync(request.Email);
        if (isEmailExists is not null)
            return UserErrors.EmailAlreadyExists;

        var user = request.Adapt<ApplicationUser>();

        var result = await userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            logger.LogInformation("confirmation code {code}", code);
            
            await SendConfirmationEmail(user, code);

            return null;
        }

        var error = result.Errors.FirstOrDefault();
        return new Error(error!.Code , error.Description , StatusCodes.Status400BadRequest);
    }


    public async Task<Error?> ConfirmEmailAsync(ConfirmEmailRequest request)
    {
        var user = await userManager.FindByIdAsync(request.UserId);
        if (user is null)
            return UserErrors.InvalidCode;

        if (user.EmailConfirmed)
            return UserErrors.EmailAlreadyConfirmed;
        try
        {
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
            var result = await userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return null;
            var error = result.Errors.FirstOrDefault();
            return new Error(error!.Code, error.Description, StatusCodes.Status400BadRequest);
        }
        catch (FormatException)
        {
            return UserErrors.InvalidCode;
        }
        
    }

    public async Task<Error?> ResendConfirmationEmailAsync(ResendConfirmationEmail request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return UserErrors.InvalidEmailOrPassword;
        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        logger.LogInformation("confirmation code {code}", code);

        await SendConfirmationEmail(user, code);
        return null;
    }

    public async Task<Error?> SendResetPasswordCodeAsync(string email)
    {
        if (await userManager.FindByEmailAsync(email) is not { } user)
            return null ;

        if (!user.EmailConfirmed)
            return UserErrors.EmailNotConfirmed;


        var code = await userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        logger.LogInformation("confirmation code {code}", code);
        BackgroundJob.Enqueue(() => SendPasswordResetEmail(user, code));
        return null;
    }

    public async Task<Error?> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        IdentityResult result;
        try
        {
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
            result = await userManager.ResetPasswordAsync(user! , code, request.NewPassword);
        }
        catch (FormatException)
        {
            result = IdentityResult.Failed(userManager.ErrorDescriber.InvalidToken());
        }
        if (result.Succeeded)
            return null;

        var error = result.Errors.FirstOrDefault();
        return new Error(error!.Code, error.Description, StatusCodes.Status401Unauthorized);
    }

    public async Task SendConfirmationEmail(ApplicationUser user , string code)
    {
        var origin = httpContextAccessor.HttpContext?.Request.Headers.Origin;

        var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",
            new Dictionary<string, string>
            {
                { "{{name}}" , user.FirstName },
                { "{{action_url}}" , $"{origin}/auth/emailConfirmation?userId={user.Id}&code={code}" }
            });
        await emailSender.SendEmailAsync(user.Email! , "✅ Survey Basket: Email Confirmation" , emailBody);
    }
    
    public async Task SendPasswordResetEmail(ApplicationUser user , string code)
    {
        var origin = httpContextAccessor.HttpContext?.Request.Headers.Origin;

        var emailBody = EmailBodyBuilder.GenerateEmailBody("ForgetPassword",
            new Dictionary<string, string>
            {
                { "{{name}}" , user.FirstName },
                { "{{action_url}}" , $"{origin}/auth/PasswordReset?userId={user.Id}&code={code}" }
            });
        await emailSender.SendEmailAsync(user.Email! , "✅ Survey Basket: Password Reset", emailBody);
    }


    
    private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

    
}
