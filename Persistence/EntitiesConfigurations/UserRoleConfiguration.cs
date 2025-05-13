using Microsoft.AspNetCore.Identity;

namespace Survey_Basket.Persistence.EntitiesConfigurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.HasData([
            new IdentityUserRole<string>
            {
                RoleId = "92b75286-d8f8-4061-9995-e6e23ccdee94",
                UserId = "6dc6528a-b280-4770-9eae-82671ee81ef7"
            },
            new IdentityUserRole<string>
            {
                RoleId = "9eaa03df-8e4f-4161-85de-0f6e5e30bfd4",
                UserId = "865d97a7-def7-4711-8f4c-05eb72bd97e1"
            }]
        );
    }
}
