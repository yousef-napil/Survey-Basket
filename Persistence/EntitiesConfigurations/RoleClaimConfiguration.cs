using Survey_Basket.Abstractions.Consts;

namespace Survey_Basket.Persistence.EntitiesConfigurations;

public class RoleClaimConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
    {
        //Default Data
        //var permissions = Permissions.GetAllPermissions();
        //var adminClaims = new List<IdentityRoleClaim<string>>();
        //for (var i = 0; i < permissions.Count; i++)
        //{
        //    adminClaims.Add(new IdentityRoleClaim<string>
        //    {
        //        Id = i + 1,
        //        ClaimType = Permissions.Type,
        //        ClaimValue = permissions[i],
        //        RoleId = DefaultRoles.AdminRoleId
        //    });
        //}

        //builder.HasData(adminClaims);

        var adminClaims = new List<IdentityRoleClaim<string>>
            {
                new IdentityRoleClaim<string> { Id = 1, ClaimType = "permissions", ClaimValue = "polls:read", RoleId = DefaultRoles.AdminRoleId },
                new IdentityRoleClaim<string> { Id = 2, ClaimType = "permissions", ClaimValue = "polls:add", RoleId = DefaultRoles.AdminRoleId },
                new IdentityRoleClaim<string> { Id = 3, ClaimType = "permissions", ClaimValue = "polls:update", RoleId = DefaultRoles.AdminRoleId },
                new IdentityRoleClaim<string> { Id = 4, ClaimType = "permissions", ClaimValue = "polls:delete", RoleId = DefaultRoles.AdminRoleId },
                new IdentityRoleClaim<string> { Id = 5, ClaimType = "permissions", ClaimValue = "questions:read", RoleId = DefaultRoles.AdminRoleId },
                new IdentityRoleClaim<string> { Id = 6, ClaimType = "permissions", ClaimValue = "questions:add", RoleId = DefaultRoles.AdminRoleId },
                new IdentityRoleClaim<string> { Id = 7, ClaimType = "permissions", ClaimValue = "questions:update", RoleId = DefaultRoles.AdminRoleId },
                new IdentityRoleClaim<string> { Id = 8, ClaimType = "permissions", ClaimValue = "users:read", RoleId = DefaultRoles.AdminRoleId },
                new IdentityRoleClaim<string> { Id = 9, ClaimType = "permissions", ClaimValue = "users:add", RoleId = DefaultRoles.AdminRoleId },
                new IdentityRoleClaim<string> { Id = 10, ClaimType = "permissions", ClaimValue = "users:update", RoleId = DefaultRoles.AdminRoleId },
                new IdentityRoleClaim<string> { Id = 11, ClaimType = "permissions", ClaimValue = "roles:read", RoleId = DefaultRoles.AdminRoleId },
                new IdentityRoleClaim<string> { Id = 12, ClaimType = "permissions", ClaimValue = "roles:add", RoleId = DefaultRoles.AdminRoleId },
                new IdentityRoleClaim<string> { Id = 13, ClaimType = "permissions", ClaimValue = "roles:update", RoleId = DefaultRoles.AdminRoleId },
                new IdentityRoleClaim<string> { Id = 14, ClaimType = "permissions", ClaimValue = "results:read", RoleId = DefaultRoles.AdminRoleId }
            };

        builder.HasData(adminClaims);



    }
}
