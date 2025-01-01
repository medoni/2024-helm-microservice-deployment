using Microsoft.Extensions.DependencyInjection;
using POS.Domains.Payment.Service.Services.PaymentProcessor;
using POS.Domains.Payment.Service.Services.PaymentProvider.Paypal;

namespace POS.Domains.Payment.Service;
/// <summary>
///
/// </summary>
public static class PaymentStartup
{
    /// <summary>
    ///
    /// </summary>
    public static IServiceCollection AddPaymentSupport(this IServiceCollection services)
    {
        services.AddPaymentProcessorSupport();
        services.AddPaypalPaymentSupport();

        return services;
    }
}
