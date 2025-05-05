namespace Survey_Basket.Contracts.Results;

public record UserVotes(
    string UserName,
    IEnumerable<QuestionAnswers> QuestionAnswers
);
