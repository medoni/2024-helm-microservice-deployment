using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POS.Domains.Payment.Service.Domain.Models;
using POS.Domains.Payment.Service.Services.PaymentProvider;
using POS.Persistence.PostgreSql.Abstractions;
using POS.Shared.Domain.Generic.Dtos;

namespace POS.Persistence.PostgreSql.Data.Payment;
/// <summary>
/// State representing a Payment
/// </summary>
public class PaymentEntity : IEntity<Guid>
{
    /// <summary>
    /// Id of the payment
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Current state of the payment.
    /// </summary>
    public PaymentStates State { get; set; }

    /// <summary>
    /// The used payment provider
    /// </summary>
    public PaymentProviderTypes PaymentProvider { get; set; }

    /// <summary>
    /// The entity type that should be paid.
    /// </summary>
    public EntityTypes EntityType { get; set; }

    /// <summary>
    /// The id of the entity that should be paid.
    /// </summary>
    public string EntityId { get; set; }

    /// <summary>
    /// Date and time when the payment was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Date and time when the payment was last changed at.
    /// </summary>
    public DateTimeOffset LastChangedAt { get; set; }

    /// <summary>
    /// Date and time when the payment was requested.
    /// </summary>
    public DateTimeOffset? RequestedAt { get; set; }

    /// <summary>
    /// Description for the payment.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Total amount of the payment.
    /// </summary>
    public GrossNetPriceDto TotalAmount { get; set; }

    /// <summary>
    /// Allowed links on the payment.
    /// </summary>
    public ICollection<PaymentLinkDescription> Links { get; set; } = new List<PaymentLinkDescription>();

    /// <summary>
    /// Payload for the underlying payment provider.
    /// </summary>
    public string? PaymentProviderPayload { get; set; }

    [Obsolete("For deserializing only.")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public PaymentEntity()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
    }

    public PaymentEntity(
        Guid id,
        DateTimeOffset createdAt,
        DateTimeOffset lastChangedAt,
        PaymentStates state,
        PaymentProviderTypes paymentProvider,
        EntityTypes entityType,
        string entityId,
        string description,
        GrossNetPriceDto totalAmount
    )
    {
        Id = id;
        CreatedAt = createdAt;
        LastChangedAt = lastChangedAt;
        State = state;
        PaymentProvider = paymentProvider;
        EntityType = entityType;
        EntityId = entityId;
        Description = description;
        TotalAmount = totalAmount;
    }
}

internal class PaymentEntityConfiguration : IEntityTypeConfiguration<PaymentEntity>
{
    public void Configure(EntityTypeBuilder<PaymentEntity> builder)
    {
        // table
        builder.ToTable("Payments", WellKnownSchemas.PaymentSchema);
        builder.HasKey(x => x.Id);

        // properties
        builder.Property(x => x.Id);

        builder.Property(x => x.CreatedAt);

        builder.Property(x => x.LastChangedAt);

        builder.Property(x => x.State)
            .HasConversion(v => v.ToString(), v => Enum.Parse<PaymentStates>(v));

        builder.Property(x => x.PaymentProvider)
            .HasConversion(v => v.ToString(), v => Enum.Parse<PaymentProviderTypes>(v));

        builder.Property(x => x.EntityType)
            .HasConversion(v => v.ToString(), v => Enum.Parse<EntityTypes>(v));

        builder.Property(x => x.EntityId);

        builder.Property(x => x.Description);

        builder.OwnsOne(x => x.TotalAmount, b =>
        {
            b.Property(x => x.Currency).HasColumnName("Currency");
            b.Property(x => x.Gross).HasColumnName("TotalAmountGross");
            b.Property(x => x.Net).HasColumnName("TotalAmountNet");
            b.Property(x => x.Vat).HasColumnName("TotalAmountVat");
        });

        builder.Property(x => x.PaymentProviderPayload);

        builder.Property(x => x.RequestedAt);

        builder.Property(x => x.Links).HasJsonConversion();
    }
}
