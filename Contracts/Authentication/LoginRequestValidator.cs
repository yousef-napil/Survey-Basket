namespace Survey_Basket.Contracts.Authentication;

public class LoginRequestValidator : AbstractValidator<loginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(x => x.Password)
            .NotEmpty();   
    }
}
