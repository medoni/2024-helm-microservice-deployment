using POS.Domains.Payment.Service.Domain.Models;
using POS.Domains.Payment.Service.Services.PaymentProvider;
using POS.Shared.Domain.Events;
using POS.Shared.Domain.Generic.Dtos;
using System.Diagnostics.CodeAnalysis;

namespace POS.Domains.Payment.Service.Domain.Events;

/// <summary>
/// Event, raised when the payment was created
/// </summary>
public class PaymentCreatedEvent : PaymentStateChangedEvent, IDomainEvent
{
    /// <summary>
    /// The used payment provider
    /// </summary>
    public required PaymentProviderTypes PaymentProvider { get; init; }

    /// <summary>
    /// The entity type that should be payed.
    /// </summary>
    public required EntityTypes EntityType { get; init; }

    /// <summary>
    /// The id of the entity that should be payed.
    /// </summary>
    public required string EntityId { get; init; }

    /// <summary>
    /// Date and time when the payment was created.
    /// </summary>
    public required DateTimeOffset CreatedAt { get; init; }

    /// <summary>
    /// Description for the payment.
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// Total amount of the payment.
    /// </summary>
    public required GrossNetPriceDto TotalAmount { get; init; }

    /// <summary>
    /// Creates a new <see cref="PaymentCreatedEvent"/>.
    /// </summary>
    [SetsRequiredMembers]
    public PaymentCreatedEvent(
        Guid id,
        PaymentProviderTypes paymentProvider,
        EntityTypes entityType,
        string entityId,
        DateTimeOffset createdAt,
        string description,
        GrossNetPriceDto totalAmount
    )
    : base(id, PaymentStates.Created, createdAt)
    {
        PaymentProvider = paymentProvider;
        EntityType = entityType;
        EntityId = entityId ?? throw new ArgumentNullException(nameof(entityId));
        CreatedAt = createdAt;
        Description = description ?? throw new ArgumentNullException(nameof(description));
        TotalAmount = totalAmount ?? throw new ArgumentNullException(nameof(totalAmount));
    }
}
