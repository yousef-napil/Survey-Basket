namespace Survey_Basket.Errors;

public static class UserErrors
{
    public static readonly Error InvalidEmailOrPassword = new("InvalidEmailOrPassword", "Invalid Email Or Password" , StatusCodes.Status400BadRequest);

}

