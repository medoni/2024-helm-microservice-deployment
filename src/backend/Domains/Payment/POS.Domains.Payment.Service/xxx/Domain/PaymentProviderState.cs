using POS.Domains.Payment.Service.Dtos;
using POS.Domains.Payment.Service.Processors.Paypal;
using POS.Shared.Domain.Generic.Dtos;
using System.Text.Json.Serialization;

namespace POS.Domains.Payment.Service.Domain;
/// <summary>
/// Persistable state of the payment provider.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(PaypalPaymentProviderState), nameof(PaypalPaymentProviderState))]
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

    /// <summary>
    /// Date and time when the request was approved.
    /// </summary>
    public DateTimeOffset? ApprovedAt { get; set; }

    /// <summary>
    /// Date and time when the payment was captured.
    /// </summary>
    public DateTimeOffset? CapturedAt { get; set; }

    /// <summary>
    /// Information about the payer.
    /// </summary>
    public PayerDto? Payer { get; set; }
}
