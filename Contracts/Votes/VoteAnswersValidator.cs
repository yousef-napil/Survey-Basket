namespace Survey_Basket.Contracts.Votes;

public class VoteAnswersValidator : AbstractValidator<VoteAnswersRequest>
{
    public VoteAnswersValidator()
    {
        RuleFor(x => x.QuestionId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.AnswerId)
            .NotEmpty()
            .GreaterThan(0);
    }
}
