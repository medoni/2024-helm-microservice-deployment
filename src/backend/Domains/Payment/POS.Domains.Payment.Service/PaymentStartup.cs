using Microsoft.Extensions.DependencyInjection;
using POS.Domains.Payment.Service.Services.PaymentProcessor;

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

        return services;
    }
}
