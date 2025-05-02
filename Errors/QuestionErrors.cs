namespace Survey_Basket.Errors;

public static class QuestionErrors
{
    public static readonly Error NotFound = new(
        "Question.NotFound",
        "The requested question could not be found.",
        StatusCodes.Status404NotFound);

    public static readonly Error AlreadyExists = new(
        "Question.AlreadyExists",
        "A question with the same content or identifier already exists.",
        StatusCodes.Status400BadRequest);

    public static readonly Error CreationFailed = new(
        "Question.CreationFailed",
        "An error occurred while creating the question. Please review the input and try again.",
        StatusCodes.Status400BadRequest);

    public static readonly Error UpdateFailed = new(
        "Question.UpdateFailed",
        "The question could not be updated. Ensure the question exists and the data is valid.",
        StatusCodes.Status400BadRequest);

    public static readonly Error DeletionFailed = new(
        "Question.DeletionFailed",
        "The question could not be deleted. It may not exist or may be linked to other data.",
        StatusCodes.Status400BadRequest);
}
