namespace Survey_Basket.Contracts.Results;

public record VotesResponse(
    string Title,
    IEnumerable<UserVotes> UserVotes
);
