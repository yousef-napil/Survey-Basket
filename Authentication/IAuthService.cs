using Survey_Basket.Contracts.Authentication;

namespace Survey_Basket.Authentication;

public interface IAuthService
{
    Task<OneOf<AuthResponse, Error>> GetTokenAsync(string email, string password , CancellationToken cancellationToken = default);
    Task<AuthResponse?> GetRefreshTokenAsync(string token, string refreshToken , CancellationToken cancellationToken = default);
    Task<bool> RevokeRefreshTokenAsync(string token, string refreshToken , CancellationToken cancellationToken = default);
    
}
