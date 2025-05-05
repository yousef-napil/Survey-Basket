using System.Security.Claims;

namespace Survey_Basket.Abstractions;

public static class UserExtension
{
    public static string? GetUserId(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.NameIdentifier);
    }

}
