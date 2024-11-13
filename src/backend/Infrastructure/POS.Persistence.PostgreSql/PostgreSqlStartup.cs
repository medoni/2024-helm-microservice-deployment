using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using POS.Domains.Customer.Persistence.Menus;
using POS.Persistence.PostgreSql.Data;
using POS.Persistence.PostgreSql.HealthChecks;
using POS.Persistence.PostgreSql.Repositories;
using POS.Shared.Persistence.PostgreSql;

namespace POS.Persistence.PostgreSql;

/// <summary>
/// Extension methods to add PostgreSql support to an application.
/// </summary>
public static class PostgreSqlStartup
{
    /// <summary>
    /// Configures the PostgreSql for the given <see cref="IServiceCollection"/>.
    /// </summary>
    public static IServiceCollection ConfigurePostgreSql(
        this IServiceCollection services,
        string connectionString
    )
    {
        services.AddDbContext<POSDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddTransient<IMenuRespository, PostgresMenuRepository>();

        services.AddUnitOfWorkSupport<POSDbContext>();

        return services;
    }

    /// <summary>
    /// Adds Health checks for the PostgreSql database.
    /// </summary>
    public static IHealthChecksBuilder AddPOSDbHealthCheck(
        this IHealthChecksBuilder builder
    )
    {
        builder.AddCheck<POSDbContextHealthCheck>("POS DB");
        return builder;
    }
}
