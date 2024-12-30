using POS.Domains.Customer.Domain.Orders;
using POS.Domains.Payment.Service.Domain;

namespace POS.Domains.Payment.Service.Services.PaymentProvider.Paypal;

/// <summary>
/// Responsible for processing payments for <see cref="Order"/>-aggregates
/// </summary>
internal class PaypalPaymentOrderProvider : IPaymentProvider
{
    public Task<PaymentAggregate> RequestPaymentAsync(
        string entityId,
        DateTimeOffset requestAt
    )
    {
        throw new NotImplementedException();
    }
}
