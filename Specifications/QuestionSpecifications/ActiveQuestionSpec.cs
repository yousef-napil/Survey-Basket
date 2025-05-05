using Survey_Basket.Contracts.Answer;
using Survey_Basket.Contracts.Question;

namespace Survey_Basket.Specifications.QuestionSpecifications;

public class ActiveQuestionSpec : BaseSpecification<Question>
{
    public ActiveQuestionSpec(int pollId)
    {
        AddCriteria(x => x.PollId == pollId && x.IsActive );
        AddInclude(x => x.Answers.Where(a => a.IsActive));
        
    }

}
