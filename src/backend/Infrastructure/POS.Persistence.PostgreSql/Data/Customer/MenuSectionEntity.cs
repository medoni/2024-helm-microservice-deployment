using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POS.Persistence.PostgreSql.Abstractions;

namespace POS.Persistence.PostgreSql.Data.Customer;

/// <summary>
/// Entity that represents a Menu section.
/// </summary>
public class MenuSectionEntity : IEntity<Guid>
{
    public Guid Id { get; set; }

    public Guid MenuId { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<MenuItemEntity> Items { get; set; } = null!;

    [Obsolete("For deserializing only.")]
    public MenuSectionEntity()
    {
    }

    public MenuSectionEntity(
        Guid id,
        Guid menuId,
        string name,
        ICollection<MenuItemEntity> items
    )
    {
        Id = id;
        MenuId = menuId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Items = items ?? throw new ArgumentNullException(nameof(items));
    }
}

internal class MenuSectionEntityConfiguration : IEntityTypeConfiguration<MenuSectionEntity>
{
    public void Configure(EntityTypeBuilder<MenuSectionEntity> builder)
    {
        // table
        builder.ToTable("MenuSections", WellKnownSchemas.CustomerSchema);
        builder.HasKey(x => x.Id);

        // properties
        builder.Property(x => x.Id);
        builder.Property(x => x.MenuId);

        builder.Property(x => x.Name);

        // relations
        builder
            .HasMany(x => x.Items)
            .WithOne()
            .HasForeignKey(x => x.MenuSectionId)
            .HasPrincipalKey(x => x.Id);
    }
}
