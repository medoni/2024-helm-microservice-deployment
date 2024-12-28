using Microsoft.EntityFrameworkCore;
using POS.Domains.Payment.Service.Domain;
using POS.Persistence.PostgreSql.Data.Customer;

#nullable disable

namespace POS.Persistence.PostgreSql.Data;

/// <summary>
/// Database context for Pizza-Ordering.
/// </summary>
public class POSDbContext : DbContext
{
    public DbSet<MenuEntity> Menus { get; set; }

    public DbSet<CartEntity> Carts { get; set; }

    public DbSet<OrderEntity> Orders { get; set; }

    public DbSet<PaymentEntity> Payments { get; set; }

    public POSDbContext(DbContextOptions options) : base(options)
    {
    }

    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

#if DEBUG
        optionsBuilder.EnableSensitiveDataLogging();
#endif
    }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // configure others entities
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}
