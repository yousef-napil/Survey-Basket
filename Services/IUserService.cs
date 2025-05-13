using Survey_Basket.Contracts.User;

namespace Survey_Basket.Services;

public interface IUserService
{
    Task<OneOf<UserProfileResponse, Error>> GetUserProfileAsync(string userId);
    Task<Error?> UpdateUserProfileAsync(string userId , ProfileUpdateRequest request);
    Task<Error?> ChangePasswordAsync(string userId , ChangePasswordRequest request);
}
