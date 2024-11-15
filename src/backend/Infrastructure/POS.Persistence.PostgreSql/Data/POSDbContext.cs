using Microsoft.EntityFrameworkCore;
using POS.Persistence.PostgreSql.Data.Customer;

#nullable disable

namespace POS.Persistence.PostgreSql.Data;

/// <summary>
/// Database context for Pizza-Ordering.
/// </summary>
public class POSDbContext : DbContext
{
    /// <summary>
    /// Menu entities.
    /// </summary>
    public DbSet<MenuEntity> Menus { get; set; }

    /// <summary>
    /// Creates a new <see cref="POSDbContext"/>.
    /// </summary>
    public POSDbContext(DbContextOptions options) : base(options)
    {
    }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // configure others entities
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}
