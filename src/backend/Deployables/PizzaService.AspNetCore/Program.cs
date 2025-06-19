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
                var allowedHosts = (
                    configuration["AllowedHosts"] ?? "*"
                ).Split(',', StringSplitOptions.RemoveEmptyEntries);

                policy.WithOrigins(allowedHosts)
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        return services;
    }
}
