using Google.Cloud.Diagnostics.AspNetCore3;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using PizzaService.Base;
using PizzaService.Base.Services.AspNet;
using PizzaService.Base.Services.HealthChecks;
using PizzaService.Base.Services.Swagger;
using POS.Domains.Customer.Persistence.FireStore;
using POS.Domains.Customer.Persistence.FireStore.Configurations;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.ConfigureGCloudTracing();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerServices();

        builder.Services.AddPizzaServiceSupport();

        builder.Services.AddHealthChecks();

        builder.Services.AddPizzaServiceFireStoreSupport(builder.Configuration);

        var app = builder.Build();

        app.UsePathBaseFromConfiguration(builder.Configuration);
        app.ConfigureSwagger(builder.Configuration);
        app.MapControllers();

        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = JsonResponseWriter.WriteResponse
        });

        await app.RunAsync();
    }
}

internal static class ProgramExtension
{
    internal static IServiceCollection AddPizzaServiceFireStoreSupport(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<CustomerFireStoreSettings>(options =>
        {
            configuration.Bind("Gcp:FireStore", options);
        });

        services.AddCustomerFireStoreSupport();
        return services;
    }

    internal static WebApplicationBuilder ConfigureGCloudTracing(
        this WebApplicationBuilder appBuilder
    )
    {
        var configuration = appBuilder.Configuration;
        var projectId = configuration.GetValue<string>("Gcp:Tracing:ProjectId");

        appBuilder.Services.AddGoogleDiagnosticsForAspNetCore(
            projectId
        );

        return appBuilder;
    }
}
