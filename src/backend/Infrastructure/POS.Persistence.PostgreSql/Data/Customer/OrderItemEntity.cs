using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POS.Persistence.PostgreSql.Abstractions;
using POS.Shared.Domain.Generic.Dtos;

namespace POS.Persistence.PostgreSql.Data.Customer;

/// <summary>
/// Entity for customer order items.
/// </summary>
public class OrderItemEntity : IEntity<Guid>
{

    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid CartItemId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public PriceInfoDto UnitPrice { get; set; } = null!;
    public int Quantity { get; set; }
    public GrossNetPriceDto TotalPrice { get; set; } = null!;

    [Obsolete("For deserializing only.")]
    public OrderItemEntity()
    {
    }

    public OrderItemEntity(
        Guid id,
        Guid orderId,
        Guid cartItemId,
        string name,
        string description,
        PriceInfoDto unitPrice,
        int quantity,
        GrossNetPriceDto totalPrice
    )
    {
        Id = id;
        OrderId = orderId;
        CartItemId = cartItemId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        UnitPrice = unitPrice ?? throw new ArgumentNullException(nameof(unitPrice));
        Quantity = quantity;
        TotalPrice = totalPrice ?? throw new ArgumentNullException(nameof(totalPrice));
    }
}

internal class OrderItemEntityConfiguration : IEntityTypeConfiguration<OrderItemEntity>
{
    public void Configure(EntityTypeBuilder<OrderItemEntity> builder)
    {
        // table
        builder.ToTable("OrderItems", WellKnownSchemas.CustomerSchema);
        builder.HasKey(x => x.Id);

        // properties
        builder.Property(x => x.Id);
        builder.Property(x => x.OrderId);
        builder.Property(x => x.CartItemId);

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

        builder.OwnsOne(x => x.TotalPrice, b =>
        {
            b.Property(x => x.Gross).HasColumnName("TotalPriceGross");
            b.Property(x => x.Net).HasColumnName("TotalPriceNet");
            b.Property(x => x.Vat).HasColumnName("TotalPriceVat");
            b.Property(x => x.Currency).HasColumnName("TotalPriceCurrency");
        });

        builder
            .HasOne<CartItemEntity>()
            .WithMany()
            .HasForeignKey(x => x.CartItemId)
            .HasPrincipalKey(x => x.Id)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
