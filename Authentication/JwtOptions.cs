using System.ComponentModel.DataAnnotations;

namespace Survey_Basket.Authentication;

public class JwtOptions
{
    [Required]
    public string Jwt { get; init; } = "Jwt";

    [Required]
    public string Key { get; init; } = string.Empty;

    [Required]
    public string Issuer { get; init; } = string.Empty;

    [Required]
    public string Audience { get; init; } = string.Empty;

    [Range(1,int.MaxValue) , Required]
    public int ExpireTime { get; init; }
}
