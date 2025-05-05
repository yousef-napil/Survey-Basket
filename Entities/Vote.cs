﻿namespace Survey_Basket.Entities;

public class Vote : BaseEntity
{
    public int PollId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime SubmittedOn { get; set; } = DateTime.UtcNow;

    public ApplicationUser User { get; set; } = default!;
    public Poll Poll { get; set; } = default!;
    public ICollection<VoteAnswer> VoteAnswers { get; set; } = [];
}
