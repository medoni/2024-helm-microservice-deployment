using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POS.Persistence.PostgreSql.Abstractions;
using POS.Shared.Domain.Generic.Dtos;

namespace POS.Persistence.PostgreSql.Data.Customer;

/// <summary>
/// Entity for cart item.
/// </summary>
public class CartItemEntity : IEntity<Guid>
{
    public Guid Id { get; set; }

    public Guid CartId { get; set; }

    public Guid MenuItemId { get; set; }

    public CartEntity Cart { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset LastChangedAt { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public PriceInfoDto UnitPrice { get; set; } = null!;

    public int Quantity { get; set; }

    [Obsolete("For deserializing only.")]
    public CartItemEntity()
    {
    }

    public CartItemEntity(
        Guid id,
        Guid cartId,
        Guid menuItemId,
        DateTimeOffset createdAt,
        DateTimeOffset lastChangedAt,
        string name,
        string description,
        PriceInfoDto unitPrice,
        int quantity
    )
    {
        Id = id;
        CartId = cartId;
        MenuItemId = menuItemId;
        CreatedAt = createdAt;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        UnitPrice = unitPrice ?? throw new ArgumentNullException(nameof(unitPrice));
        LastChangedAt = lastChangedAt;
        Quantity = quantity;
    }
}

internal class CartItemEntityConfiguration : IEntityTypeConfiguration<CartItemEntity>
{
    public void Configure(EntityTypeBuilder<CartItemEntity> builder)
    {
        // table
        builder.ToTable("CartItems", WellKnownSchemas.CustomerSchema);
        builder.HasKey(x => x.Id);

        // properties
        builder.Property(x => x.Id);

        builder.Property(x => x.MenuItemId);

        builder.Property(x => x.CreatedAt);

        builder.Property(x => x.LastChangedAt);

        builder.Property(x => x.Name);

        builder.Property(x => x.Description);

        builder.OwnsOne(x => x.UnitPrice, b =>
        {
            b.OwnsOne(x => x.Price, b =>
            {
                b.Property(x => x.Gross).HasColumnName("UnitPriceGross");
                b.Property(x => x.Net).HasColumnName("UnitPriceNet");
                b.Property(x => x.Vat).HasColumnName("UnitPriceVat");
                b.Property(x => x.Currency).HasColumnName("UnitPriceCurrency");
            });
            b.Property(x => x.RegularyVatInPercent).HasColumnName("UnitPriceRegVatPercent");
        });

        builder.Property(x => x.Quantity);

        // indexes
        builder.HasIndex(x => new { x.Id, x.MenuItemId });
    }
}
