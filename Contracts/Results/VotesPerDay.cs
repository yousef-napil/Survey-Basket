namespace Survey_Basket.Contracts.Results;

public record VotesPerDay(
    DateOnly Date,
    int NumberOfVotes
);
