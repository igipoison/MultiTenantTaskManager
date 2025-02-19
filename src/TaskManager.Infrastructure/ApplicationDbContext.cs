using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using TaskManager.Domain;

namespace TaskManager.Infrastructure;

public interface IApplicationDbContext
{
    void Migrate();
}

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options), IApplicationDbContext
{
    public DbSet<Tenant> Tenants { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Tenant>().HasIndex(t=>t.Domain).IsUnique();
    }

    public void Migrate()
    {
        Database.Migrate();
    }
}