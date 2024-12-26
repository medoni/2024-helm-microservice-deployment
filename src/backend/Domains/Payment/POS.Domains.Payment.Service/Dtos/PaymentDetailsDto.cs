using POS.Domains.Payment.Service.Domain;
using POS.Shared.Domain.Generic.Dtos;

namespace POS.Domains.Payment.Service.Dtos;
/// <summary>
/// Dto containing details for a payment
/// </summary>
public class PaymentDetailsDto
{
    /// <summary>
    /// Id of the payment.
    /// </summary>
    public required Guid PaymentId { get; init; }

    /// <summary>
    /// ID of the entity for which the payment should be requested.
    /// </summary>
    public required string EntityId { get; init; }

    /// <summary>
    /// Type of the entity for which the payment should be requested
    /// </summary>
    public required EntityTypes EntityType { get; init; }

    /// <summary>
    /// Date and time when the request was started.
    /// </summary>
    public required DateTimeOffset RequestedAt { get; init; }

    /// <summary>
    /// State of the payment
    /// </summary>
    public required PaymentStates State { get; init; }

    /// <summary>
    /// Payment provider.
    /// </summary>
    public required PaymentProviders Provider { get; init; }

    /// <summary>
    /// Description of the payment
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// Amount of the payment.
    /// </summary>
    public required GrossNetPriceDto Amount { get; init; }

    /// <summary>
    /// Allowed links for the payment
    /// </summary>
    public required List<PaymentLinkDescription> Links { get; init; }

    /// <summary>
    /// Date and time when the request was successfully paid.
    /// </summary>
    public DateTimeOffset? PayedAt { get; init; }
}
