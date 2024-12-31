using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using PizzaService.AspNetCore.Services.EventPublisher;
using PizzaService.Base;
using PizzaService.Base.Services.HealthChecks;
using PizzaService.Base.Services.Swagger;
using POS.Domains.Payment.Api;
using POS.Domains.Payment.Service.Services.PaymentProvider.Paypal;
using POS.Persistence.PostgreSql;
using POS.Shared.Infrastructure.PubSub.Abstractions;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerServices();

        builder.Services.AddPizzaServiceSupport();

        builder.Services.AddPaypalPaymentSupport()
            .Configure<PaypalPaymentSettings>(x => builder.Configuration.Bind("PaypalApi", x));

        builder.Services.AddHealthChecks()
            .AddPOSDbHealthCheck();

        builder.Services.AddPOSDb(builder.Configuration);
        builder.Services.AddSingleton<IEventPublisher, NullEventPublisher>();

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
