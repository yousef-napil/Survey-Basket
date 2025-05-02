namespace Survey_Basket.Entities;

public sealed class Answer : AuditableEntity
{
    public string Content { get; set; } = string.Empty;
    public int QuestionId { get; set; }
    public bool IsActive { get; set; } = true;


    public Question Question { get; set; } = default!;
}
