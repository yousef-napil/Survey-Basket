using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Survey_Basket.Abstractions;
using Survey_Basket.Contracts.User;
using Survey_Basket.Services;

namespace Survey_Basket.Controller;
[Route("[controller]")]
[ApiController]
[Authorize]
public class AccountController(IUserService userService) : ControllerBase
{
    private readonly IUserService userService = userService;

    [HttpGet("info")]
    public async Task<IActionResult> UserProfile()
    {
        var user = await userService.GetUserProfileAsync(User.GetUserId()!);
        return user.Match(
            Ok,
            error => error.ToProblem(error.StatusCode)
        );
    }

    [HttpPut("info")]
    public async Task<IActionResult> UpdateUserProfile([FromBody] ProfileUpdateRequest request)
    {
        var result = await userService.UpdateUserProfileAsync(User.GetUserId()!, request);
        if (result is null)
            return NoContent();
        return result.ToProblem(result.StatusCode);
    }

    [HttpPut("password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var result = await userService.ChangePasswordAsync(User.GetUserId()!, request);
        if (result is null)
            return NoContent();
        return result.ToProblem(result.StatusCode);
    }
}
