using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using POS.Domains.Customer.Persistence.Menus;
using POS.Persistence.PostgreSql.Data;
using POS.Persistence.PostgreSql.HealthChecks;
using POS.Persistence.PostgreSql.Repositories;
using POS.Shared.Persistence.PostgreSql;

namespace POS.Persistence.PostgreSql;

public static class PostgreSqlStartup
{
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

    public static IHealthChecksBuilder AddPOSDbHealthCheck(
        this IHealthChecksBuilder builder
    )
    {
        builder.AddCheck<POSDbContextHealthCheck>("POS DB");
        return builder;
    }
}
