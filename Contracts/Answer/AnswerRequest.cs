namespace Survey_Basket.Contracts.Answer;

public record AnswerRequest(
        string Content,
        int QuestionId,
        bool IsActive
    );
