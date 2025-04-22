
namespace Survey_Basket.Persistence.EntitiesConfigurations;

public class AuthConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.OwnsMany(r => r.refreshTokens)
            .ToTable("RefreshTokens")
            .WithOwner()
            .HasForeignKey("UserId");


        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(50);
    }
}
