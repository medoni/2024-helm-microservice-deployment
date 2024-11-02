using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace PizzaOrderingService.Services.Swagger;

internal static class SwaggerStartup
{
    internal static IServiceCollection AddSwaggerServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.ExampleFilters();
        });

        return services;
    }

    internal static WebApplication ConfigureSwagger(
        this WebApplication app,
        IConfiguration configuration
    )
    {
        if (configuration.GetValue<bool>("Swagger:Enabled"))
        {
            app.ConfigureSwaggerApp();
        }

        return app;
    }

    internal static WebApplication ConfigureSwaggerApp(this WebApplication app)
    {
        app.UseSwagger(c =>
        {
            c.PreSerializeFilters.Add((swaggerDoc, request) =>
            {
                if (request.Headers.TryGetValue("X-Forwarded-Prefix", out var serverPath))
                {
                    swaggerDoc.Servers = new List<OpenApiServer>()
                    {
                        new OpenApiServer() { Description = "Pizza-Ordering-Service", Url = serverPath }
                    };
                }
            });
        });
        app.UseSwaggerUI();

        return app;
    }
}
