namespace Survey_Basket.Entities;

[Owned]
public sealed class RefreshToken
{
    public string Token { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedOn { get; set; }
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive => RevokedOn is null && !IsExpired;
}
