using Microsoft.Extensions.DependencyInjection;

namespace POS.Domains.Payment.Service.Services.PaymentProcessor;
/// <summary>
///
/// </summary>
public static class PaymentProcessorStartup
{
    /// <summary>
    ///
    /// </summary>
    public static IServiceCollection AddPaymentProcessorSupport(this IServiceCollection services)
    {
        services.AddTransient<IPaymentProcessor, DefaultPaymentProcessor>();

        return services;
    }
}
