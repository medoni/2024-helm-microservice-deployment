using POS.Domains.Payment.Service.Domain;

namespace POS.Domains.Payment.Service.Services.PaymentProvider;

/// <summary>
/// Definition for a payment provider for a given entity type.
/// </summary>
public interface IPaymentProvider
{
    /// <summary>
    /// Requests a payment for the given entity id
    /// </summary>
    Task<PaymentAggregate> RequestPaymentAsync(
        string entityId,
        DateTimeOffset requestAt
    );
}
