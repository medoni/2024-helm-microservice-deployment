using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace POS.Persistence.PostgreSql.Data.Customer;

/// <summary>
/// Entity for Menu.
/// </summary>
public class MenuEntity
{
    /// <summary>
    /// Id of the Menu.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Date and time when the Menu was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Date and time when the Menu was last changed at.
    /// </summary>
    public DateTimeOffset LastChangedAt { get; set; }

    /// <summary>
    /// True, when the Menu is active.
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Date and time when the menu was activated.
    /// </summary>
    public DateTimeOffset? ActivatedAt { get; set; }

    /// <summary>
    /// List of sections
    /// </summary>
    public ICollection<MenuSectionEntity> Sections { get; set; } = null!;

    /// <summary>
    /// Creates a new <see cref="MenuEntity"/>.
    /// </summary>
    [Obsolete("For deserializing only.")]
    public MenuEntity()
    {

    }

    /// <summary>
    /// Creates a new <see cref="MenuEntity"/>.
    /// </summary>
    public MenuEntity(
        Guid id,
        DateTimeOffset createdAt,
        DateTimeOffset lastChangedAt,
        bool? isActive,
        DateTimeOffset? activatedAt,
        ICollection<MenuSectionEntity> sections
    )
    {
        Id = id;
        CreatedAt = createdAt;
        LastChangedAt = lastChangedAt;
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
