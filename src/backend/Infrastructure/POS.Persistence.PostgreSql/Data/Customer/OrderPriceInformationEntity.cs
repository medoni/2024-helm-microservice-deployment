using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POS.Shared.Domain.Generic.Dtos;

namespace POS.Persistence.PostgreSql.Data.Customer;
/// <summary>
/// Entity for price information about the customer order
/// </summary>
public class OrderPriceInformationEntity
{
    public Guid OrderId { get; set; }
    public GrossNetPriceDto TotalItemPrice { get; set; } = null!;
    public GrossNetPriceDto TotalPrice { get; set; } = null!;
    public GrossNetPriceDto DeliverCosts { get; set; } = null!;
    public GrossNetPriceDto Discount { get; set; } = null!;

    [Obsolete("For deserializing only.")]
    public OrderPriceInformationEntity()
    {
    }

    public OrderPriceInformationEntity(
        Guid orderId,
        GrossNetPriceDto totalItemPrice,
        GrossNetPriceDto totalPrice,
        GrossNetPriceDto deliverCosts,
        GrossNetPriceDto discount
    )
    {
        OrderId = orderId;
        TotalItemPrice = totalItemPrice ?? throw new ArgumentNullException(nameof(totalItemPrice));
        TotalPrice = totalPrice ?? throw new ArgumentNullException(nameof(totalPrice));
        DeliverCosts = deliverCosts ?? throw new ArgumentNullException(nameof(deliverCosts));
        Discount = discount ?? throw new ArgumentNullException(nameof(discount));
    }
}

internal class OrderPriceInformationEntityConfiguration : IEntityTypeConfiguration<OrderPriceInformationEntity>
{
    public void Configure(EntityTypeBuilder<OrderPriceInformationEntity> builder)
    {
        // table
        builder.ToTable("OrderPriceInfos", WellKnownSchemas.CustomerSchema);
        builder.HasKey(x => x.OrderId);

        // properties
        builder.Property(x => x.OrderId);

        builder.OwnsOne(x => x.TotalItemPrice, b =>
        {
            b.Property(x => x.Gross).HasColumnName("TotalItemPriceGross");
            b.Property(x => x.Net).HasColumnName("TotalItemPriceNet");
            b.Property(x => x.Vat).HasColumnName("TotalItemPriceVat");
            b.Property(x => x.Currency).HasColumnName("TotalItemPriceCurrency");
        });

        builder.OwnsOne(x => x.TotalPrice, b =>
        {
            b.Property(x => x.Gross).HasColumnName("TotalPriceGross");
            b.Property(x => x.Net).HasColumnName("TotalPriceNet");
            b.Property(x => x.Vat).HasColumnName("TotalPriceVat");
            b.Property(x => x.Currency).HasColumnName("TotalPriceCurrency");
        });

        builder.OwnsOne(x => x.DeliverCosts, b =>
        {
            b.Property(x => x.Gross).HasColumnName("DeliverCostsGross");
            b.Property(x => x.Net).HasColumnName("DeliverCostsNet");
            b.Property(x => x.Vat).HasColumnName("DeliverCostsVat");
            b.Property(x => x.Currency).HasColumnName("DeliverCostsCurrency");
        });

        builder.OwnsOne(x => x.Discount, b =>
        {
            b.Property(x => x.Gross).HasColumnName("DiscountGross");
            b.Property(x => x.Net).HasColumnName("DiscountNet");
            b.Property(x => x.Vat).HasColumnName("DiscountVat");
            b.Property(x => x.Currency).HasColumnName("DiscountCurrency");
        });
    }
}
