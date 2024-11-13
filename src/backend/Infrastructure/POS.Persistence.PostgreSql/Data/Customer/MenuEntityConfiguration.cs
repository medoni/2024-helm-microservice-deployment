using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POS.Domains.Customer.Domain.Menus.Entities;

namespace POS.Persistence.PostgreSql.Data.Customer;
internal class MenuEntityConfiguration : IEntityTypeConfiguration<MenuEntity>
{
    public void Configure(EntityTypeBuilder<MenuEntity> builder)
    {
        // table
        builder.ToTable("Menu", WellKnownSchemas.CustomerSchema);
        builder.HasKey(x => x.MenuId);

        // properties
        builder.Property(x => x.MenuId);

        builder.Property(x => x.CreatedAt);

        builder.Property(x => x.LastChangedAt);

        builder.Property(x => x.IsActive);

        builder.Property(x => x.ActivatedAt);

        builder
            .Property(x => x.Sections)
            .HasColumnType("varchar")
            .HasJsonConversion();

        // indexes
        builder
            .HasIndex(x => x.IsActive)
            .IsUnique();
    }
}
