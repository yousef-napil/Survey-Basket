namespace Survey_Basket.Entities;

public sealed class Poll : AuditableEntity
{
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public bool IsPublished { get; set; } = default!;
    public DateOnly StartsAt { get; set; }
    public DateOnly EndsAt { get; set; }

    public ICollection<Question> Questions { get; set; } = [];
    public ICollection<Vote> Votes { get; set; } = [];
}
