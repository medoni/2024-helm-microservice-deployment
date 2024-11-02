using Microsoft.EntityFrameworkCore;
using POS.Domains.Customer.Domain.Menus.Dtos;

#nullable disable

namespace POS.Persistence.PostgreSql.Data;
public class POSDbContext : DbContext
{
    public DbSet<MenuEntity> Menus { get; set; }

    public POSDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // the following entities are used as Owns... relations.
        modelBuilder.Ignore<MenuSectionEntity>();
        modelBuilder.Ignore<MenuItemEntity>();

        // configure others entities
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}
