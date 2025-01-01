using POS.Domains.Payment.Service.Domain.Models;
using POS.Domains.Payment.Service.Services.PaymentProvider;
using POS.Shared.Domain.Events;
using POS.Shared.Domain.Generic.Dtos;
using System.Diagnostics.CodeAnalysis;

namespace POS.Domains.Payment.Service.Domain.Events;
/// <summary>
/// Event, raised when a payment was requested.
/// </summary>
public class PaymentRequestedEvent : PaymentStateChangedEvent, IDomainEvent
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
    /// Date and time when the payment was requested.
    /// </summary>
    public required DateTimeOffset RequestedAt { get; init; }

    /// <summary>
    /// Total amount of the payment.
    /// </summary>
    public required GrossNetPriceDto TotalAmount { get; init; }

    /// <summary>
    /// Links of the payment.
    /// </summary>
    public required List<PaymentLinkDescription> Links { get; init; }

    /// <summary>
    /// Payload of the payment provider.
    /// </summary>
    public required string PaymentProviderPayload { get; init; }

    /// <summary>
    /// Creates a new <see cref="PaymentRequestedEvent"/>.
    /// </summary>
    [SetsRequiredMembers]
    public PaymentRequestedEvent(
        Guid id,
        PaymentProviderTypes paymentProvider,
        EntityTypes entityType,
        string entityId,
        DateTimeOffset requestedAt,
        GrossNetPriceDto totalAmount,
        List<PaymentLinkDescription> links,
        string paymentProviderPayload
    )
    : base(id, PaymentStates.Created, requestedAt)
    {
        PaymentProvider = paymentProvider;
        EntityType = entityType;
        EntityId = entityId ?? throw new ArgumentNullException(nameof(entityId));
        RequestedAt = requestedAt;
        TotalAmount = totalAmount ?? throw new ArgumentNullException(nameof(totalAmount));
        Links = links;
        PaymentProviderPayload = paymentProviderPayload;
    }
}
