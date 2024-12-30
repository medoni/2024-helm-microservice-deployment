using Microsoft.Extensions.DependencyInjection;
using POS.Domains.Payment.Service.Domain;

namespace POS.Domains.Payment.Service.Services.PaymentProvider.Paypal;

internal static class PaypalPaymentProviderStartup
{
    public static IServiceCollection AddPaypalPaymentSupport(this IServiceCollection services)
    {
        services.AddKeyedTransient<IPaymentProvider, PaypalPaymentOrderProvider>((PaymentProviderTypes.Paypal, EntityTypes.CustomerOrder));
        return services;
    }
}
