namespace Survey_Basket.Contracts.User;

public record ChangePasswordRequest(
    string OldPassword,
    string NewPassword
);