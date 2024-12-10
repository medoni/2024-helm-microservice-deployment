using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using POS.Shared.Persistence.PostgreSql.UnitOfWork;
using POS.Shared.Persistence.UOW;

namespace POS.Shared.Persistence.PostgreSql;

/// <summary>
/// Extension methods to add PostgreSql persistence to a <see cref="IServiceCollection"/>.
/// </summary>
public static class PostgreSqlStartup
{
    /// <summary>
    /// Adds Unit-Of-Work support for a given <see cref="DbContext"/> to the <see cref="IServiceCollection"/>.
    /// </summary>
    public static IServiceCollection AddUnitOfWorkSupport<TDbContext>(
        this IServiceCollection services
    )
    where TDbContext : DbContext
    {
        services.AddUnitOfWork<EfCoreUnitOfWork>();

        return services;
    }
}
