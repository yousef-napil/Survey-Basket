
using Survey_Basket.Contracts.User;

namespace Survey_Basket.Services;

public class UserService(UserManager<ApplicationUser> userManager) : IUserService
{
    private readonly UserManager<ApplicationUser> userManager = userManager;

    

    public async Task<OneOf<UserProfileResponse , Error>> GetUserProfileAsync(string userId)
    {
        var user = await userManager.Users
            .Where(x => x.Id == userId)
            .ProjectToType<UserProfileResponse>()
            .FirstOrDefaultAsync();

        if (user is null)
            return UserErrors.UserNotFound;

        return user;
    }

    public async Task<Error?> UpdateUserProfileAsync(string userId , ProfileUpdateRequest request)
    {
        var result = await userManager.Users
            .Where(x => x.Id == userId)
            .ExecuteUpdateAsync(x => x
                .SetProperty(u => u.FirstName, request.FirstName)
                .SetProperty(u => u.LastName, request.LastName));

        if (result == 0)
            return UserErrors.UserUpdateFailed;
        return null;
    }

    public async Task<Error?> ChangePasswordAsync(string userId, ChangePasswordRequest request)
    {
        var user = await userManager.FindByIdAsync(userId);

        var result = await userManager.ChangePasswordAsync(user!, request.OldPassword, request.NewPassword);

        if (result.Succeeded)
            return null;

        var error = result.Errors.FirstOrDefault();
        return new Error(error!.Code , error.Description , StatusCodes.Status400BadRequest);
    }
}
