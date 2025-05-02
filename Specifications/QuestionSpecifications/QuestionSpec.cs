using Survey_Basket.Contracts.Answer;
using Survey_Basket.Contracts.Question;

namespace Survey_Basket.Specifications.QuestionSpecifications;

public class QuestionSpec : BaseSpecification<Question>
{
    public QuestionSpec(int pollId)
    {
        AddCriteria(x => x.PollId == pollId);
        AddInclude(x => x.Answers);
        
    }

    public QuestionSpec(int pollId, int questionId)
    {
        AddCriteria(x => x.PollId == pollId && x.Id == questionId);
        AddInclude(x => x.Answers);
    }
}
