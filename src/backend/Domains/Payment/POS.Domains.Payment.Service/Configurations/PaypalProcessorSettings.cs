using PaypalServerSdk.Standard.Models;
using POS.Domains.Payment.Service.Processors.Paypal;

namespace POS.Domains.Payment.Service.Configurations;
/// <summary>
/// Settings for <see cref="PaypalPaymentProcessor"/>
/// </summary>
public record PaypalProcessorSettings
{
    /// <summary>
    /// Access key for the Paypal-Api
    /// </summary>
    public required string ApiAccessKey { get; init; }

    /// <summary>
    /// Secret key for the Paypal-Api
    /// </summary>
    public required string ApiSecretKey { get; init; }

    /// <summary>
    /// Return url for successful payment
    /// </summary>
    public required string ReturnUrl { get; init; }

    /// <summary>
    /// Return url for canceled payment
    /// </summary>
    public required string CancelUrl { get; init; }

    /// <summary>
    /// Settings of Payee
    /// </summary>
    public required Payee Payee { get; init; }
}
