namespace Survey_Basket.Entities;

public sealed class Question : AuditableEntity
{
    public string Content { get; set; } = string.Empty;
    public int PollId { get; set; }
    public bool IsActive { get; set; } = true;


    public Poll Poll { get; set; } = default!;
    public List<Answer> Answers { get; set; } = new();
}
