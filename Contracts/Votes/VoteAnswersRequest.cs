namespace Survey_Basket.Contracts.Votes;

public record VoteAnswersRequest(
    int QuestionId,
    int AnswerId
);
