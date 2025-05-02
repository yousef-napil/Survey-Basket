namespace Survey_Basket.Persistence.EntitiesConfigurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.HasIndex(q => new { q.PollId , q.Content }).IsUnique();

        builder.Property(q => q.Content)
            .IsRequired()
            .HasMaxLength(1000);

    }
}

