using Microsoft.Extensions.DependencyInjection;
using POS.Domains.Payment.Service.Domain;
using POS.Domains.Payment.Service.Services.PaymentProvider;

namespace POS.Domains.Payment.Service.Services.PaymentProcessor;
internal class DefaultPaymentProcessor(
    IServiceProvider serviceProvider
) : IPaymentProcessor
{

    public async Task<PaymentAggregate> RequestPaymentAsync(
        PaymentProviderTypes paymentProviderType,
        EntityTypes entityType,
        string entityId,
        DateTimeOffset requestAt
    )
    {
        var paymentProvider = serviceProvider.GetRequiredKeyedService<IPaymentProvider>((paymentProviderType, entityType));
        var aggregate = await paymentProvider.RequestPaymentAsync(
            entityId,
            requestAt
        );

        return aggregate;
    }
}
