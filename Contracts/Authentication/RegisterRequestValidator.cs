using Survey_Basket.Abstractions.Consts;

namespace Survey_Basket.Contracts.Authentication;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Length(3,100);
        RuleFor(x => x.LastName)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .Length(3, 100);

        RuleFor(x => x.Password)
            .NotEmpty()
            .Length(8, 100)
            .Matches(RegularExpressions.Password)
            .WithMessage("pass");

    }
}

