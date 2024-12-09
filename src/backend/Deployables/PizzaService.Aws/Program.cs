using Amazon.XRay.Recorder.Core;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using PizzaService.Aws.Services.AspNet;
using PizzaService.Base;
using PizzaService.Base.Services.HealthChecks;
using PizzaService.Base.Services.Swagger;
using POS.Persistence.PostgreSql;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerServices();
        builder.Services.AddProblemDetails();
        builder.Services.AddLogging(options =>
        {
            options.AddJsonConsole(format =>
            {
                format.IncludeScopes = true;
                format.UseUtcTimestamp = true;
            });
        });

        builder.Services.ConfigureTracing(builder.Configuration);

        builder.Services.AddPizzaServiceSupport();

        builder.Services.AddHealthChecks()
            .AddPOSDbHealthCheck();

        builder.Services.AddPOSDb(builder.Configuration);

        var app = builder.Build();

        app.UsePathBase("/api");
        app.UseXRay("PizzaService");
        app.ConfigureSwagger(builder.Configuration);
        app.MapControllers();
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = JsonResponseWriter.WriteResponse
        });
        //app.UseMiddleware<StripPathMiddleware>();
        app.UseMiddleware<RequestLoggingMiddleware>();

        await app.RunAsync();
    }
}

internal static class ProgramExtension
{
    internal static IServiceCollection AddPOSDb(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString = configuration.GetValue<string>("PizzaDbContext:Connection")!;

        services.ConfigurePostgreSql(connectionString);
        return services;
    }

    internal static IServiceCollection ConfigureTracing(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        AWSXRayRecorder.InitializeInstance(configuration);

        return services;
    }
}
