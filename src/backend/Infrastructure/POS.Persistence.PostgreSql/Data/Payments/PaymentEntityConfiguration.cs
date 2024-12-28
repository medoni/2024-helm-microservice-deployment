using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POS.Domains.Payment.Service.Domain;

namespace POS.Persistence.PostgreSql.Data.Payments;

internal class PaymentEntityConfiguration : IEntityTypeConfiguration<PaymentEntity>
{
    public void Configure(EntityTypeBuilder<PaymentEntity> builder)
    {
        // table
        builder.ToTable("Payments", WellKnownSchemas.PaymentsSchema);
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => new { x.EntityType, x.EntityId });

        // properties
        builder.Property(x => x.Id);

        builder.Property(x => x.EntityType);
        builder.Property(x => x.EntityId);

        builder.Property(x => x.RequestedAt);

        builder.Property(x => x.Provider)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<PaymentProviders>(v)
            );

        builder.Property(x => x.State)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<PaymentStates>(v)
            );

        builder.Property(x => x.PayedAt);

        builder.Property(x => x.ProviderState).HasJsonConversion();
    }
}
