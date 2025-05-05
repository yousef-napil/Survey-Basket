using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Survey_Basket.Abstractions;
using Survey_Basket.Authentication;
using Survey_Basket.Contracts.Authentication;

namespace Survey_Basket.Controller;
[Route("[controller]")]
[ApiController]

public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService authService = authService;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] loginRequest request)
    {
        var authResponse = await authService.GetTokenAsync(request.Email, request.Password);
        return authResponse.Match(
                Ok,
                error => Problem(statusCode: StatusCodes.Status401Unauthorized,
                    title: error.Title,
                    detail: error.Description
                )
            );
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var authResponse = await authService.RegisterAsync(request);
        if (authResponse is null)
            return Ok("User registered successfully");
        return authResponse.ToProblem(authResponse.StatusCode);
    }

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(ConfirmEmailRequest request)
    {
        var user = await authService.ConfirmEmailAsync(request);
        if (user is null)
            return Ok("User Email confirmed");
        return user.ToProblem(user.StatusCode);
    }

    [HttpPost("Reset-confirm-email")]
    public async Task<IActionResult> ResetConfirmEmail([FromBody] ResendConfirmationEmail request)
    {
        var user = await authService.ResendConfirmationEmailAsync(request);
        if (user is null)
            return Ok("Confirmation Email Sent successfully");
        return user.ToProblem(user.StatusCode);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var authResponse = await authService.GetRefreshTokenAsync(request.Token, request.RefreshToken);
        if (authResponse == null)
        {
            return BadRequest("Invalid Token");
        }
        return Ok(authResponse);
    }

    [HttpPost("revoke-refresh-token")]
    public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenRequest request)
    {
        var result = await authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken);
        if (!result)
        {
            return BadRequest("Invalid Token");
        }
        return Ok("Token revoked successfully");
    }
}
