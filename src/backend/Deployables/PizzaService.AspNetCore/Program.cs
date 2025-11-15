using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using PizzaService.Base;
using PizzaService.Base.Services.HealthChecks;
using PizzaService.Base.Services.Swagger;
using POS.Persistence.PostgreSql;

internal static class Program
{
    private const string CorsPolicyName = "CorsPolicy";

    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerServices();

        builder.Services.AddPizzaServiceSupport();

        builder.Services.AddHealthChecks()
            .AddPOSDbHealthCheck();

        builder.Services.AddCorsSupport(CorsPolicyName, builder.Configuration);

        builder.Services.AddPOSDb(builder.Configuration);

        var app = builder.Build();

        app.ConfigureSwagger(builder.Configuration);
        app.MapControllers();
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = JsonResponseWriter.WriteResponse
        });
        app.UseCors(CorsPolicyName);


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

    internal static IServiceCollection AddCorsSupport(
        this IServiceCollection services,
        string policyName,
        IConfiguration configuration
    )
    {
        services.AddCors(options =>
        {
            options.AddPolicy(policyName, policy =>
            {
                var allowedHostsConfig = configuration["AllowedHosts"];

                if (string.IsNullOrWhiteSpace(allowedHostsConfig))
                {
                    throw new InvalidOperationException(
                        "CORS configuration error: 'AllowedHosts' is not configured. " +
                        "Please set 'AllowedHosts' in appsettings.json or environment variables."
                    );
                }

                var allowedHosts = allowedHostsConfig
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                policy.WithOrigins(allowedHosts)
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        return services;
    }
}
