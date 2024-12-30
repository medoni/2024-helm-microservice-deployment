using POS.Domains.Payment.Service.Domain;

namespace POS.Domains.Payment.Service.Services.PaymentProvider;
internal interface IPaymentProvider
{
    Task<PaymentAggregate> RequestPaymentAsync(
        string entityId,
        DateTimeOffset requestAt
    );
}
