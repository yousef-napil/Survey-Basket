using Survey_Basket.Contracts.Answer;

namespace Survey_Basket.Contracts.Question;

public record QuestionResponse(
        int Id,
        string Content,
        List<AnswerResponse> Answers
);
