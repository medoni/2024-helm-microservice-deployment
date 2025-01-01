using POS.Domains.Payment.Service.Domain.Models;
using POS.Domains.Payment.Service.Services.PaymentProvider;

namespace POS.Domains.Payment.Api.Dtos;

/// <summary>
/// Dto to request a payment for a specific entity.
/// </summary>
public class RequestPaymentDto
{
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
    /// Payment provider
    /// </summary>
    public required PaymentProviderTypes Provider { get; init; }
}
