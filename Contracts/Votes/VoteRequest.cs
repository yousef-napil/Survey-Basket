namespace Survey_Basket.Contracts.Votes;

public record VoteRequest(
    List<VoteAnswersRequest> VoteAnswers
);
