using Asp.Versioning;

namespace PizzaOrderingService.Services.Apis;

internal static class ApisStartup
{
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
