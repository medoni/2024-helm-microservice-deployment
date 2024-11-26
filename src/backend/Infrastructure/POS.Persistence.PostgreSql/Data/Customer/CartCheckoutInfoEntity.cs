using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace POS.Persistence.PostgreSql.Data.Customer;
public class CartCheckoutInfoEntity
{
    public required Guid CartId { get; set; }
    public required Guid OrderId { get; set; }
    public required DateTimeOffset CheckedOutAt { get; set; }
}

internal class CartCheckoutInfoEntityConfiguration : IEntityTypeConfiguration<CartCheckoutInfoEntity>
{
    public void Configure(EntityTypeBuilder<CartCheckoutInfoEntity> builder)
    {
        // table
        builder.ToTable("CartCheckoutInfos", WellKnownSchemas.CustomerSchema);
        builder.HasKey(x => x.CartId);

        // properties
        builder.Property(x => x.OrderId);

        builder.Property(x => x.CheckedOutAt);

        builder
            .HasOne<OrderEntity>()
            .WithMany()
            .HasForeignKey(x => x.OrderId)
            .HasPrincipalKey(x => x.Id);
    }
}
