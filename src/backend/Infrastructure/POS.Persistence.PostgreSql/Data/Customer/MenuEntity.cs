using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POS.Persistence.PostgreSql.Abstractions;

namespace POS.Persistence.PostgreSql.Data.Customer;

/// <summary>
/// Entity for Menu.
/// </summary>
public class MenuEntity : IEntity<Guid>
{
    public Guid Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset LastChangedAt { get; set; }

    public string Currency { get; set; } = null!;

    public bool? IsActive { get; set; }

    public DateTimeOffset? ActivatedAt { get; set; }

    public ICollection<MenuSectionEntity> Sections { get; set; } = null!;

    [Obsolete("For deserializing only.")]
    public MenuEntity()
    {

    }

    public MenuEntity(
        Guid id,
        DateTimeOffset createdAt,
        DateTimeOffset lastChangedAt,
        string currency,
        bool? isActive,
        DateTimeOffset? activatedAt,
        ICollection<MenuSectionEntity> sections
    )
    {
        Id = id;
        CreatedAt = createdAt;
        LastChangedAt = lastChangedAt;
        Currency = currency ?? throw new ArgumentNullException(nameof(currency));
        IsActive = isActive;
        ActivatedAt = activatedAt;
        Sections = sections ?? throw new ArgumentNullException(nameof(sections));
    }
}

internal class MenuEntityConfiguration : IEntityTypeConfiguration<MenuEntity>
{
    public void Configure(EntityTypeBuilder<MenuEntity> builder)
    {
        // table
        builder.ToTable("Menus", WellKnownSchemas.CustomerSchema);
        builder.HasKey(x => x.Id);

        // properties
        builder.Property(x => x.Id);

        builder.Property(x => x.CreatedAt);

        builder.Property(x => x.LastChangedAt);

        builder.Property(x => x.Currency);

        builder.Property(x => x.IsActive);

        builder.Property(x => x.ActivatedAt);

        // indexes
        builder
            .HasIndex(x => x.IsActive)
            .IsUnique();

        // relations
        builder
            .HasMany(x => x.Sections)
            .WithOne()
            .HasForeignKey(x => x.MenuId)
            .HasPrincipalKey(x => x.Id);
    }
}
