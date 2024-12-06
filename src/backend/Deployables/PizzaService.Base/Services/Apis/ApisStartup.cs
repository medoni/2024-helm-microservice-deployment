using Asp.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace PizzaService.Base.Services.Apis;

/// <summary>
/// Extension methods to add api versioning to the <see cref="IServiceCollection"/>.
/// </summary>
public static class ApisStartup
{
    /// <summary>
    /// Adds api versioning to the <see cref="IServiceCollection"/>.
    /// </summary>
    public static IServiceCollection AddApiVersioning(IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1);
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader()
            );
        });

        return services;
    }
}
