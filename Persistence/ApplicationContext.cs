using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Survey_Basket.Persistence;

public class ApplicationContext(DbContextOptions<ApplicationContext> options , IHttpContextAccessor httpContext) : IdentityDbContext<ApplicationUser>(options)
{
    private readonly IHttpContextAccessor httpContext = httpContext;

    public DbSet<Poll> Polls => Set<Poll>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<Answer> Answers => Set<Answer>();
    public DbSet<Vote> Votes => Set<Vote>();
    public DbSet<VoteAnswer> VoteAnswers => Set<VoteAnswer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        var entityTypes = modelBuilder.Model.GetEntityTypes();
        entityTypes
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade && !fk.IsOwnership)
            .ToList()
            .ForEach(fk => fk.DeleteBehavior = DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {

        var entries = ChangeTracker.Entries<AuditableEntity>();
        var userId = httpContext.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedById = userId;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedOn = DateTime.UtcNow;
                    entry.Entity.UpdatedById = userId;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

}

