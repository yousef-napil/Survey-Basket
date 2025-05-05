namespace Survey_Basket.Errors;

public static class VoteErrors
{
    public static readonly Error NotFound = new(
        "Vote.NotFound",
        "The requested Vote could not be found.",
        StatusCodes.Status404NotFound);

    public static readonly Error AlreadyVoted = new(
        "Vote.AlreadyVoted",
        "The user has already voted for this poll.",
        StatusCodes.Status400BadRequest);
    
    public static readonly Error InvalidVote = new(
        "Vote.InvalidVote",
        "The vote is invalid.",
        StatusCodes.Status400BadRequest);

    public static readonly Error SaveFailed = new(
        "Vote.SaveFailed",
        "The vote hasn't been saved.",
        StatusCodes.Status500InternalServerError);

    public static readonly Error NoVotes = new(
        "Vote.NoVotes",
        "There are no votes for this poll.",
        StatusCodes.Status404NotFound);



}
