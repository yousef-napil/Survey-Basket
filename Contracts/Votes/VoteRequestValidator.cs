namespace Survey_Basket.Contracts.Votes;

public class VoteRequestValidator : AbstractValidator<VoteRequest>
{
    public VoteRequestValidator()
    {
        RuleFor(x => x.VoteAnswers)
            .NotEmpty();

        RuleForEach(x => x.VoteAnswers).SetInheritanceValidator(v => v.Add(new VoteAnswersValidator()));
    }
}
