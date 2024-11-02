using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using POS.Domains.Customer.UseCases;
using POS.Persistence.PostgreSql;
using POS.Persistence.PostgreSql.Data;
using POS.Shared.Persistence.PostgreSql.DbSeeds;

namespace POS.Persistence.DbSeed;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var configRoot = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

        var services = new ServiceCollection()
            .AddCustomerUseCases()
            .AddPOSDb(configRoot);

        services
            .AddSeederSupport<POSDbContext>()
            .AddSeederFromAssembly(typeof(Program).Assembly);

        services
            .AddLogging(options =>
            {
                options.AddJsonConsole();
            });

        var svcp = services.BuildServiceProvider();
        var seederProcessor = svcp.GetRequiredService<ISeederProcessor>();
        await seederProcessor.ProcessAsync(svcp);
    }

    internal static IServiceCollection AddPOSDb(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString = configuration.GetValue<string>("CONNECTION_STRING")!;

        services.ConfigurePostgreSql(connectionString);
        return services;
    }
}
