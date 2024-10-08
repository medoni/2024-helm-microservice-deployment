﻿using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using PizzaOrderingService.Data;
using PizzaOrderingService.Services.HealthChecks;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers();
        builder.Services.AddHealthChecks()
            .AddCheck<VersionInfoHealthCheck>("VersionInfo")
            .AddCheck<PizzaDbContextHealthCheck>("PizzaDbContext");

        builder.Services.AddPizzaDbContext(
            builder.Configuration
        );

        var app = builder.Build();

        app.ConfigureSwagger(builder.Configuration);
        app.MapControllers();
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = JsonResponseWriter.WriteResponse
        });

        await app.InitializePizzaDbAsync(builder.Configuration);

        await app.RunAsync();
    }

    internal static WebApplication ConfigureSwagger(
        this WebApplication app,
        IConfiguration configuration
    )
    {
        if (configuration.GetValue<bool>("Swagger:Enabled"))
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        return app;
    }

    internal static async Task InitializePizzaDbAsync(
        this WebApplication app,
        IConfiguration configuration
    )
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PizzaDbContext>();

        if (configuration.GetValue<bool>("PizzaDbContext:MigrateOnStartup"))
        {
            await MigrateDatabaseAsync(dbContext);
        }


        if (configuration.GetValue<bool>("PizzaDbContext:SeedExampleData"))
        {
            await DatabaseSeeder.SeedDatabaseAsync(dbContext);
        }
    }

    internal static async Task MigrateDatabaseAsync(PizzaDbContext dbContext)
    {
        await dbContext.Database.MigrateAsync();
    }
}
