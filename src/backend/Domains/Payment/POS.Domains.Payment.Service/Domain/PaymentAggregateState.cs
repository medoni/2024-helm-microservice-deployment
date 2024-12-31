using POS.Domains.Payment.Service.Domain.Models;
using POS.Domains.Payment.Service.Services.PaymentProvider;
using POS.Shared.Domain.Generic.Dtos;

namespace POS.Domains.Payment.Service.Domain;
/// <summary>
/// State representing a Payment
/// </summary>
public record PaymentAggregateState
{
    /// <summary>
    /// Id of the payment
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// Current state of the payment.
    /// </summary>
    public required PaymentStates State { get; set; }

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
    /// Date and time when the payment was requested.
    /// </summary>
    public DateTimeOffset? RequestedAt { get; set; }

    /// <summary>
    /// Description for the payment.
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// Total amount of the payment.
    /// </summary>
    public required GrossNetPriceDto TotalAmount { get; set; }

    /// <summary>
    /// Allowed links on the payment.
    /// </summary>
    public List<PaymentLinkDescription> Links { get; set; } = new();

    /// <summary>
    /// Payload for the underlaying payment provider.
    /// </summary>
    public string? PaymentProviderPayload { get; set; }
}
