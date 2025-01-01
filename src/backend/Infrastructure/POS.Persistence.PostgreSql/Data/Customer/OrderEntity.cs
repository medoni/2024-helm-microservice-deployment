using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POS.Domains.Customer.Abstractions.Orders;
using POS.Domains.Customer.Domain.Orders.Models;
using POS.Persistence.PostgreSql.Abstractions;

namespace POS.Persistence.PostgreSql.Data.Customer;
/// <summary>
/// Entity for a customer orders.
/// </summary>
public class OrderEntity : IEntity<Guid>
{
    public Guid Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset LastChangedAt { get; set; }

    public OrderStates State { get; set; }

    public ICollection<OrderItemEntity> Items { get; set; } = null!;

    public OrderPriceInformationEntity PriceSummary { get; set; } = null!;

    public PaymentInfo? PaymentInfo { get; set; }

    [Obsolete("For deserializing only.")]
    public OrderEntity()
    {
    }

    public OrderEntity(
        Guid id,
        DateTimeOffset createdAt,
        DateTimeOffset lastChangedAt,
        OrderStates state,
        ICollection<OrderItemEntity> items,
        OrderPriceInformationEntity priceSummary
    )
    {
        Id = id;
        CreatedAt = createdAt;
        LastChangedAt = lastChangedAt;
        State = state;
        Items = items ?? throw new ArgumentNullException(nameof(items));
        PriceSummary = priceSummary ?? throw new ArgumentNullException(nameof(priceSummary));
    }
}

internal class OrderEntityConfiguration : IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        // table
        builder.ToTable("Orders", WellKnownSchemas.CustomerSchema);
        builder.HasKey(x => x.Id);

        // properties
        builder.Property(x => x.Id);

        builder.Property(x => x.CreatedAt);

        builder.Property(x => x.LastChangedAt);

        builder.Property(x => x.State)
            .HasConversion(v => v.ToString(), v => Enum.Parse<OrderStates>(v));

        builder.Property(x => x.PaymentInfo)
            .HasJsonConversion();

        builder
            .HasOne(x => x.PriceSummary)
            .WithOne()
            .HasForeignKey<OrderPriceInformationEntity>(x => x.OrderId)
            .HasPrincipalKey<OrderEntity>(x => x.Id);

        builder
            .HasMany(x => x.Items)
            .WithOne()
            .HasForeignKey(x => x.OrderId)
            .HasPrincipalKey(x => x.Id);
    }
}
