using Survey_Basket.Contracts.Answer;

namespace Survey_Basket.Contracts.Question;

public record QuestionRequest(
        string Content,
        List<string> Answers
);
