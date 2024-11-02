using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POS.Persistence.PostgreSql.Abstractions;
using POS.Shared.Domain.Generic.Dtos;

namespace POS.Persistence.PostgreSql.Data.Customer;

/// <summary>
/// Entity that represents a Menu item.
/// </summary>
public class MenuItemEntity : IEntity<Guid>
{
    public Guid Id { get; set; }

    public Guid MenuSectionId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public List<string> Ingredients { get; set; } = new();

    public PriceInfoDto Price { get; set; } = null!;

    [Obsolete("For deserializing only.")]
    public MenuItemEntity()
    {
    }

    public MenuItemEntity(
        Guid id,
        Guid menuSectionId,
        string name,
        string description,
        List<string> ingredients,
        PriceInfoDto price
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
            b.OwnsOne(x => x.Price, b =>
            {
                b.Property(x => x.Gross).HasColumnName("PriceGross");
                b.Property(x => x.Net).HasColumnName("PriceNet");
                b.Property(x => x.Vat).HasColumnName("PriceVat");
                b.Property(x => x.Currency).HasColumnName("PriceCurrency");
            });
            b.Property(x => x.RegularyVatInPercent).HasColumnName("PriceRegVatPercent");
        });
    }
}
