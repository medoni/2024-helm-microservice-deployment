using Microsoft.Extensions.DependencyInjection;
using POS.Domains.Payment.Service;

namespace POS.Domains.Payment.Api;
/// <summary>
///
/// </summary>
public static class PaymentApiStartup
{
    /// <summary>
    ///
    /// </summary>
    public static IServiceCollection AddPaypalPaymentSupport(this IServiceCollection services)
    {
        services.AddPaymentServiceSupport();

        return services;
    }
}
