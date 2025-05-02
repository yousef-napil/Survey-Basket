namespace Survey_Basket.Errors;

public static class PollErrors
{
    public static readonly Error NotFound = new(
        "Poll.NotFound",
        "The requested poll could not be found.",
        StatusCodes.Status404NotFound);

    public static readonly Error AlreadyExists = new(
        "Poll.AlreadyExists",
        "A poll with the same title or identifier already exists.",
        StatusCodes.Status400BadRequest);

    public static readonly Error CreationFailed = new(
        "Poll.CreationFailed",
        "An error occurred while creating the poll. Please try again.",
        StatusCodes.Status400BadRequest);

    public static readonly Error UpdateFailed = new(
        "Poll.UpdateFailed",
        "Unable to update the poll. Please ensure the poll exists and the data is valid.",
        StatusCodes.Status400BadRequest);

    public static readonly Error DeletionFailed = new(
        "Poll.DeletionFailed",
        "The poll could not be deleted. It may not exist or is referenced by other data.",
        StatusCodes.Status400BadRequest);
}
