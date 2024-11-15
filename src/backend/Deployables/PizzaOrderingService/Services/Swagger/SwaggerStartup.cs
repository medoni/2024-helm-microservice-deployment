using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PizzaOrderingService.Services.Swagger;

internal static class SwaggerStartup
{
    internal static IServiceCollection AddSwaggerServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.ExampleFilters();

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "PizzaService Api",
                Description = "API to cover a sample ordering process for pizzas.",
                Contact = new OpenApiContact
                {
                    Name = "GitHub",
                    Url = new Uri("https://github.com/medoni/2024-helm-microservice-deployment")
                },
                License = new OpenApiLicense
                {
                    Name = "MIT License",
                    Url = new Uri("https://github.com/medoni/2024-helm-microservice-deployment/blob/master/LICENSE.TXT")
                }
            });

            IncludeXmlCommentsForApiAssemblies(options);
        });

        return services;
    }

    private static void IncludeXmlCommentsForApiAssemblies(SwaggerGenOptions options)
    {
        var directory = AppContext.BaseDirectory;
        var assemblyFiles = Directory.EnumerateFiles(directory, "*.Api.dll");

        foreach (var assemblyFile in assemblyFiles)
        {
            var docFile = Path.Combine(directory, Path.GetFileNameWithoutExtension(assemblyFile) + ".xml");
            if (File.Exists(docFile))
            {
                options.IncludeXmlComments(docFile);
            }
        }
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
                        new OpenApiServer() { Description = "PizzaService", Url = serverPath }
                    };
                }
            });
        });
        app.UseSwaggerUI();

        return app;
    }
}
