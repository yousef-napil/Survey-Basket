using System.Security.Cryptography;
using Survey_Basket.Contracts.Authentication;

namespace Survey_Basket.Authentication;

public class AuthService(UserManager<ApplicationUser> userManager
                        , IJWTProvider jwtProvider) : IAuthService
{
    private readonly UserManager<ApplicationUser> userManager = userManager;
    private readonly IJWTProvider jwtProvider = jwtProvider;
    private readonly int refreshTokenExpiryDays = 14;

    public async Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
            return null;
        var isPasswordValid = await userManager.CheckPasswordAsync(user, password);
        if (!isPasswordValid)
            return null;

        var (token, expiresIn) = jwtProvider.GenerateToken(user);

        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiryDate = DateTime.UtcNow.AddDays(refreshTokenExpiryDays);

        user.refreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            ExpiresAt = refreshTokenExpiryDate,
        });

        await userManager.UpdateAsync(user);

        return new AuthResponse(user.Id, user.FirstName , user.LastName , user.Email! , token, expiresIn , refreshToken , refreshTokenExpiryDate);
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
    private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
}
