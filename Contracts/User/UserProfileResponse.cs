﻿namespace Survey_Basket.Contracts.User;

public record UserProfileResponse(
    string Email,
    string UserName,
    string FirstName,
    string LastName
);