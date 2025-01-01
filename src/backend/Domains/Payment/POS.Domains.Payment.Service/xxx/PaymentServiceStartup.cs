using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaypalServerSdk.Standard;
using PaypalServerSdk.Standard.Authentication;
using POS.Domains.Customer.Persistence.Orders;
using POS.Domains.Payment.Service.Configurations;
using POS.Domains.Payment.Service.Domain;
using POS.Domains.Payment.Service.Processors;
using POS.Domains.Payment.Service.Processors.Paypal;

namespace POS.Domains.Payment.Service;
/// <summary>
///
/// </summary>
public static class PaymentServiceStartup
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddPaymentServiceSupport(this IServiceCollection services)
    {
        services.AddTransient<IPaymentService, DefaultPaymentService>();
        services.AddPaypalClient();

        services.AddKeyedTransient<IPaymentProcessor, PaypalPaymentProcessor>(PaymentProviders.Paypal, (svcp, key) =>
        {
            return new PaypalPaymentProcessor(
                svcp.GetRequiredService<PaypalServerSdkClient>(),
                svcp.GetRequiredService<IOptions<PaypalProcessorSettings>>().Value,
                svcp.GetRequiredService<IOrderRepository>()
            );
        });

        return services;
    }

    private static IServiceCollection AddPaypalClient(this IServiceCollection services)
    {
        services.AddSingleton<PaypalServerSdkClient>(svcp =>
        {
            var paypalSettings = svcp.GetRequiredService<IOptions<PaypalProcessorSettings>>().Value;

            var client = new PaypalServerSdkClient.Builder()
                .ClientCredentialsAuth(
                    new ClientCredentialsAuthModel.Builder(
                        paypalSettings.ApiAccessKey,
                        paypalSettings.ApiSecretKey
                    )
                    .Build())
                .Environment(PaypalServerSdk.Standard.Environment.Sandbox)
                .LoggingConfig(config => config
                    .LogLevel(LogLevel.Information)
                    .RequestConfig(reqConfig => reqConfig.Body(true))
                    .ResponseConfig(respConfig => respConfig.Headers(true))
                )
                .Build();

            return client;
        });

        return services;
    }
}
