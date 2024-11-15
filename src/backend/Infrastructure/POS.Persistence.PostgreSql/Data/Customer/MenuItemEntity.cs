using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POS.Shared.Domain.Generic.Dtos;

namespace POS.Persistence.PostgreSql.Data.Customer;

/// <summary>
/// Entity that represents a Menu item.
/// </summary>
public class MenuItemEntity
{


    /// <summary>
    /// Id of the menu item.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Id of the section where the item belongs to.
    /// </summary>
    public Guid MenuSectionId { get; set; }

    /// <summary>
    /// Name of the menu item.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Description of the menu item.
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// List of ingredients.
    /// </summary>
    public List<string> Ingredients { get; set; } = new();

    /// <summary>
    /// Unit price of the item.
    /// </summary>
    public MoneyDto Price { get; set; } = null!;

    /// <summary>
    /// Creates a new <see cref="MenuItemEntity"/>.
    /// </summary>
    [Obsolete("For deserializing only.")]
    public MenuItemEntity()
    {
    }

    /// <summary>
    /// Creates a new <see cref="MenuItemEntity"/>.
    /// </summary>
    public MenuItemEntity(
        Guid id,
        Guid menuSectionId,
        string name,
        string description,
        List<string> ingredients,
        MoneyDto price
    )
    {
        Id = id;
        MenuSectionId = menuSectionId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        Ingredients = ingredients ?? throw new ArgumentNullException(nameof(ingredients));
        Price = price ?? throw new ArgumentNullException(nameof(price));
    }
}

internal class MenuItemEntityConfiguration : IEntityTypeConfiguration<MenuItemEntity>
{
    public void Configure(EntityTypeBuilder<MenuItemEntity> builder)
    {
        // table
        builder.ToTable("MenuItems", WellKnownSchemas.CustomerSchema);
        builder.HasKey(x => x.Id);

        // properties
        builder.Property(x => x.Id);
        builder.Property(x => x.MenuSectionId);

        builder.Property(x => x.Name);
        builder.Property(x => x.Description);

        builder
            .Property(x => x.Ingredients)
            .HasJsonConversion();

        builder.OwnsOne(x => x.Price, b =>
        {
            b.Property(y => y.Amount).HasColumnName("Price");
            b.Property(y => y.Currency).HasColumnName("PriceCur");
        });
    }
}
