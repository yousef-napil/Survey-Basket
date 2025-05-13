namespace Survey_Basket.Errors;

public static class UserErrors
{
    public static readonly Error InvalidEmailOrPassword = new(
        "User.InvalidEmailOrPassword",
        "Invalid Email Or Password" , 
        StatusCodes.Status401Unauthorized);
    public static readonly Error EmailAlreadyExists = new(
        "User.EmailAlreadyExists",
        "Email Already Exists",
        StatusCodes.Status409Conflict);

    public static readonly Error EmailNotConfirmed = new(
        "User.EmailNotConfirmed",
        "Email Not Confirmed",
        StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidCode = new(
        "User.InvalidCode",
        "Invalid Code",
        StatusCodes.Status400BadRequest);

    public static readonly Error EmailAlreadyConfirmed = new(
        "User.EmailAlreadyConfirmed",
        "Email Already Confirmed",
        StatusCodes.Status400BadRequest);

    public static readonly Error UserNotFound = new(
        "User.UserNotFound",
        "User Not Found",
        StatusCodes.Status404NotFound);

    public static readonly Error UserUpdateFailed = new(
        "User.UserUpdateFailed",
        "User Update Failed",
        StatusCodes.Status400BadRequest);

}

