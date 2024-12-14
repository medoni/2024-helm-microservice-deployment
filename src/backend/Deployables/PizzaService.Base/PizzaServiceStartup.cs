using Microsoft.Extensions.DependencyInjection;
using PizzaService.Base.Services.HealthChecks;
using POS.Domains.Customer.Api;
using System.Text.Json.Serialization;

namespace PizzaService.Base;

/// <summary>
/// Extensions methods for IServiceCollection to configure PizzaService
/// </summary>
public static class PizzaServiceStartup
{
    /// <summary>
    ///
    /// </summary>
    public static IServiceCollection AddPizzaServiceSupport(this IServiceCollection services)
    {
        services.AddControllers()
            .AddCustomerApi()
            .AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
            );
        ;
        services.AddApiVersioning();
        services.AddHealthChecks()
            .AddCheck<VersionInfoHealthCheck>("VersionInfo");

        return services;
    }
}
