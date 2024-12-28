using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using POS.Domains.Customer.Persistence.Carts;
using POS.Domains.Customer.Persistence.Carts.Decorators;
using POS.Domains.Customer.Persistence.Menus;
using POS.Domains.Customer.Persistence.Menus.Decorators;
using POS.Domains.Customer.Persistence.Orders;
using POS.Domains.Customer.Persistence.Orders.Decorators;
using POS.Domains.Payment.Service;
using POS.Persistence.PostgreSql.Data;
using POS.Persistence.PostgreSql.HealthChecks;
using POS.Persistence.PostgreSql.Repositories;
using POS.Shared.Infrastructure.DependencyInjection;
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

        services
            .AddTransient<IMenuRespository, PostgresMenuRepository, LoggingMenuRepositoryDecorator>()
            .AddTransient<ICartRepository, PostgresCartRepository, LoggingCartRepositoryDecorator>()
            .AddTransient<IOrderRepository, PostgresOrderRepository, LoggingOrderRepositoryDecorator>()
            .AddTransient<IPaymentRepository, PostgresPaymentRepository>()
        ;

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
