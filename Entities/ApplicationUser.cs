
namespace Survey_Basket.Entities;

public sealed class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<RefreshToken> refreshTokens { get; set; } = [];
}

