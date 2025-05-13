using Survey_Basket.Abstractions.Consts;

namespace Survey_Basket.Contracts.User;

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.OldPassword)
            .NotEmpty();

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .Matches(RegularExpressions.Password)
            .NotEqual(x => x.OldPassword)
            .WithMessage("New Password must be Different than Old Password");
    }
}
