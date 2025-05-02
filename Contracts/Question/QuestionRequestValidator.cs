namespace Survey_Basket.Contracts.Question;

public class QuestionRequestValidator : AbstractValidator<QuestionRequest>
{
    public QuestionRequestValidator()
    {
        RuleFor(q => q.Content)
            .NotEmpty()
            .Length(3 ,1000);


        RuleFor(q => q.Answers)
            .Must(A => A.Count > 1)
            .WithMessage("Question Must has at least 2 answers");

        RuleFor(q => q.Answers)
            .Must(A => A.Distinct().Count() == A.Count)
            .WithMessage("Answers Must be Unique for Same Question");
            
    }
}
