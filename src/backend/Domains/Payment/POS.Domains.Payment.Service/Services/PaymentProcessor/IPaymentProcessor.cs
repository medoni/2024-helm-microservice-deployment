using POS.Domains.Payment.Service.Domain;
using POS.Domains.Payment.Service.Domain.Models;
using POS.Domains.Payment.Service.Services.PaymentProvider;

namespace POS.Domains.Payment.Service.Services.PaymentProcessor;
/// <summary>
/// Responsible to process payments.
/// </summary>
public interface IPaymentProcessor
{
    /// <summary>
    /// Requests a payment for an entity.
    /// </summary>
    /// <param name="paymentProvider">The payment provider that should be used.</param>
    /// <param name="entityType">The type of the entity that should be payed.</param>
    /// <param name="entityId">The id of the entity that should be payed.</param>
    /// <param name="requestAt">Date and time when the payment is requested.</param>
    Task<PaymentAggregate> RequestPaymentAsync(
        PaymentProviderTypes paymentProvider,
        EntityTypes entityType,
        string entityId,
        DateTimeOffset requestAt
    );
}
