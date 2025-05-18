using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace PizzaService.Base.Services.AspNet;

/// <summary>
/// Extension methods for adding path based routing configured via configuration.
/// </summary>
public static class ConfigurablePathBase
{
    /// <summary>
    /// Adds path based routing configured via configuration.
    /// </summary>
    public static IApplicationBuilder UsePathBaseFromConfiguration(
        this IApplicationBuilder app,
        IConfiguration configuration,
        string configPath = "App:PathBase"
    )
    {
        var value = configuration.GetValue<string?>(configPath);
        if (value is not null)
        {
            app.UsePathBase(value);
        }

        return app;
    }
}
