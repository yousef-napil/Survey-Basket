namespace Survey_Basket.Errors;

public class Error
{
    public string Title { get; }
    public string Description { get; }
    public int StatusCode { get; set; }
    public Error(string title, string description, int statusCode)
    {
        Title = title;
        Description = description;
        StatusCode = statusCode;
    }
}
