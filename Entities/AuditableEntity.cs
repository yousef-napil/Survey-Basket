namespace Survey_Basket.Entities;

public class AuditableEntity : BaseEntity
{
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedOn { get; set; }
    public string CreatedById { get; set; } = string.Empty;
    public string? UpdatedById { get; set; }

    public ApplicationUser CreatedBy { get; set; } = default!;
    public ApplicationUser UpdatedBy { get; set; } = default!;
}
