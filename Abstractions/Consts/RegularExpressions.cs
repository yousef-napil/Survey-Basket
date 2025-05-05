namespace Survey_Basket.Abstractions.Consts;

public static class RegularExpressions
{
    public const string Password = "(?=(.*[0-9]))(?=.*[\\!@#$%^&*()\\\\[\\]{}\\-_+=~`|:;\"'<>,./?])(?=.*[a-z])(?=(.*[A-Z]))(?=(.*)).{8,}";
    public const string PhoneNumber = @"^\+?[1-9]\d{1,14}$";
    public const string Url = @"^(https?|ftp)://[^\s/$.?#].[^\s]*$";
    public const string Date = @"^\d{4}-\d{2}-\d{2}$";
}
