﻿namespace Survey_Basket.Contracts.Authentication;

public record ConfirmEmailRequest(
    string UserId,
    string Code
);
