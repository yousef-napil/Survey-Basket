namespace Survey_Basket.Errors;

public static class CRUDErrors
{
    public static readonly Error NotFound = new("NotFound", "Item not found" , StatusCodes.Status404NotFound);
    public static readonly Error AlreadyExists = new("AlreadyExists", "Item already exists", StatusCodes.Status400BadRequest);
    public static readonly Error CreationFailed = new("CreationFailed", "Item Creation Failed", StatusCodes.Status400BadRequest);
    public static readonly Error UpdateFailed = new("UpdateFailed", "Item Creation Failed", StatusCodes.Status400BadRequest);
    public static readonly Error DeletionFailed = new("DeletionFailed", "Item Creation Failed", StatusCodes.Status400BadRequest);
}
