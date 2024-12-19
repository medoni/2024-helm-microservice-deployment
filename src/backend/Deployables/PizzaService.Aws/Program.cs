using Amazon.XRay.Recorder.Core;
using Amazon.XRay.Recorder.Handlers.AwsSdk;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using PizzaService.Aws.Services.AspNet;
using PizzaService.Base;
using PizzaService.Base.Services.HealthChecks;
using PizzaService.Base.Services.Swagger;
using POS.Domains.Customer.Persistence.DynamoDb;
using POS.Domains.Customer.Persistence.DynamoDb.Configurations;
using POS.Infrastructure.PubSub.Sns;

internal class Program
{
    private static async Task Main(string[] args)
    {
        AWSSDKHandler.RegisterXRayForAllServices();

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

        builder.Services.AddHealthChecks();

        builder.Services.AddPizzaServiceDynamoDbSupport(builder.Configuration);
        builder.Services.AddSnsEventPublisher(options => builder.Configuration.Bind("Aws:Sns", options));

        var app = builder
            .Build();

        app.UseXRay("PizzaService");
        app.ConfigureSwagger(builder.Configuration);
        app.MapControllers();
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = JsonResponseWriter.WriteResponse
        });
        app.UseMiddleware<RequestLoggingMiddleware>();

        await app.RunAsync();
    }
}

internal static class ProgramExtension
{
    internal static IServiceCollection AddPizzaServiceDynamoDbSupport(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<CustomerDynamoDbSettings>(options =>
        {
            configuration.Bind("Aws:DynamoDb", options);
        });

        services.AddCustomerDynamoDbSupport();
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
