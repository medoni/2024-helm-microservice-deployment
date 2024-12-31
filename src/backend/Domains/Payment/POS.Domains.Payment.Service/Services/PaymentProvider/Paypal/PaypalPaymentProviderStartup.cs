using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaypalServerSdk.Standard;
using PaypalServerSdk.Standard.Authentication;
using POS.Domains.Payment.Service.Domain.Models;

namespace POS.Domains.Payment.Service.Services.PaymentProvider.Paypal;

internal static class PaypalPaymentProviderStartup
{
    public static IServiceCollection AddPaypalPaymentSupport(this IServiceCollection services)
    {
        services.AddKeyedTransient<IPaymentProvider, PaypalPaymentOrderProvider>((PaymentProviderTypes.Paypal, EntityTypes.CustomerOrder));

        services.AddTransient<IPaypalInternalApi, PaypalInternalApi>();
        services.AddPaypalClient();

        return services;
    }

    private static IServiceCollection AddPaypalClient(this IServiceCollection services)
    {
        services.AddSingleton<PaypalServerSdkClient>(svcp =>
        {
            var paypalSettings = svcp.GetRequiredService<IOptions<PaypalPaymentSettings>>().Value;

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
