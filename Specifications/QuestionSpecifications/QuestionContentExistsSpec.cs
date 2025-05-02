namespace Survey_Basket.Specifications.QuestionSpecifications;

public class QuestionContentExistsSpec : BaseSpecification<Question>
{
    public QuestionContentExistsSpec(int pollId , string content)
    {
        AddAny(q => q.Content == content && q.PollId == pollId);
    }

    public QuestionContentExistsSpec(int pollId, string content, int questionId)
    {
        AddAny(q => q.Content == content && q.PollId == pollId && q.Id != questionId);
    }
}

