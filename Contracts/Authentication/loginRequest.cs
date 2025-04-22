namespace Survey_Basket.Contracts.Authentication;

public record loginRequest(
    string Email,
    string Password
);
