using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POS.Domains.Customer.Abstractions.Carts;
using POS.Persistence.PostgreSql.Abstractions;

namespace POS.Persistence.PostgreSql.Data.Customer;

/// <summary>
/// Entity for cart.
/// </summary>
public class CartEntity : IEntity<Guid>
{
    public Guid Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset LastChangedAt { get; set; }

    public CartStates State { get; set; }

    public Guid MenuId { get; set; }

    public string Currency { get; set; } = null!;

    public List<CartItemEntity> Items { get; set; } = new();

    public CartCheckoutInfoEntity? CheckoutInfo { get; set; }

    [Obsolete("For deserializing only.")]
    public CartEntity()
    {
    }

    public CartEntity(
        Guid id,
        DateTimeOffset createdAt,
        DateTimeOffset lastChangedAt,
        Guid menuId,
        string currency
    )
    {
        Id = id;
        CreatedAt = createdAt;
        MenuId = menuId;
        Currency = currency ?? throw new ArgumentNullException(nameof(currency));
        LastChangedAt = lastChangedAt;
    }
}

internal class CartEntityConfiguration : IEntityTypeConfiguration<CartEntity>
{
    public void Configure(EntityTypeBuilder<CartEntity> builder)
    {
        // table
        builder.ToTable("Carts", WellKnownSchemas.CustomerSchema);
        builder.HasKey(x => x.Id);

        // properties
        builder.Property(x => x.Id);

        builder.Property(x => x.CreatedAt);

        builder.Property(x => x.LastChangedAt);

        builder.Property(x => x.State)
            .HasConversion(v => v.ToString(), v => Enum.Parse<CartStates>(v));

        builder.Property(x => x.MenuId);
        builder.HasOne<MenuEntity>()
            .WithMany()
            .HasForeignKey(x => x.MenuId);

        builder.Property(x => x.Currency);

        builder
            .HasMany(x => x.Items)
            .WithOne(x => x.Cart)
            .HasForeignKey(x => x.CartId);

        builder.HasOne(x => x.CheckoutInfo)
            .WithOne()
            .HasForeignKey<CartCheckoutInfoEntity>(x => x.CartId)
            .HasPrincipalKey<CartEntity>(x => x.Id);
    }
}
