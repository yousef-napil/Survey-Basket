using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Survey_Basket.Persistence;

public class ApplicationContext(DbContextOptions<ApplicationContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Poll> Polls => Set<Poll>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

}

