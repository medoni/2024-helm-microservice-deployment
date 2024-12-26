using POS.Shared.Domain.Generic.Dtos;

namespace POS.Domains.Payment.Service.Domain;
/// <summary>
/// Perstistable state of the payment provider.
/// </summary>
public abstract class PaymentProviderState
{
    /// <summary>
    /// Internal payment Id of the payment provider.
    /// </summary>
    public required string PaymentProviderId { get; init; }

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
}
