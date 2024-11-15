using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using PizzaOrderingService.Services.HealthChecks;
using PizzaOrderingService.Services.Swagger;
using POS.Domains.Customer.Api;
using POS.Persistence.PostgreSql;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerServices();
        builder.Services.AddControllers()
            .AddCustomerApi()
        ;
        builder.Services.AddApiVersioning();
        builder.Services.AddHealthChecks()
            .AddCheck<VersionInfoHealthCheck>("VersionInfo")
            .AddPOSDbHealthCheck();

        builder.Services.AddPOSDb(builder.Configuration);

        var app = builder.Build();

        app.ConfigureSwagger(builder.Configuration);
        app.MapControllers();
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = JsonResponseWriter.WriteResponse
        });

        await app.RunAsync();
    }

    internal static IServiceCollection AddPOSDb(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString = configuration.GetValue<string>("PizzaDbContext:Connection")!;

        services.ConfigurePostgreSql(connectionString);
        return services;
    }
}
