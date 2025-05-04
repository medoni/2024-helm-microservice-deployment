using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PizzaService.Base.Services.Swagger;

/// <summary>
/// Extensions to configure Swagger for Startup.
/// </summary>
public static class SwaggerStartup
{
    /// <summary>
    /// Adds Swagger support to the service collection
    /// </summary>
    public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.ExampleFilters();
            options.CustomOperationIds(e => $"{e.ActionDescriptor.RouteValues["controller"]}_{e.ActionDescriptor.RouteValues["action"]}_{e.HttpMethod}");

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

    /// <summary>
    /// Configures Swaggers
    /// </summary>
    public static WebApplication ConfigureSwagger(
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

    /// <summary>
    /// Configures Swagger
    /// </summary>
    public static WebApplication ConfigureSwaggerApp(this WebApplication app)
    {
        app.UseSwagger(c =>
        {
            c.SerializeAsV2 = true;
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
