namespace Survey_Basket.Authentication;

public interface IJWTProvider
{
    (string token, int expiresIn) GenerateToken(ApplicationUser user);
    string? ValidateToken(string token);
}
