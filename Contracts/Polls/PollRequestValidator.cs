using FluentValidation;

namespace Survey_Basket.Contracts.Polls;

public class PollRequestValidator : AbstractValidator<PollRequest>
{
    public PollRequestValidator()
    {
        RuleFor(p => p.Title)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(p => p.Summary)
            .NotEmpty()
            .Length(5, 1500);

        RuleFor(p => p.StartsAt)
            .NotEmpty()
            .GreaterThanOrEqualTo(Today());

        RuleFor(p => p.EndsAt)
            .NotEmpty()
            .GreaterThanOrEqualTo(p => p.StartsAt);
    }
    private DateOnly Today() => DateOnly.FromDateTime(DateTime.Today);
}
