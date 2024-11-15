using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using POS.Shared.Persistence.PostgreSql.UnitOfWork;
using POS.Shared.Persistence.Repositories;
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
        services.AddTransient<UnitOfWorkFactory>(svcp =>
        {
            return () => svcp.GetRequiredService<IUnitOfWork>();
        });

        services.AddSingleton<RepositoryCache>(svcp => BuildRepositoryCache(services));

        services.AddScoped<IUnitOfWork>(svcp =>
        {
            var repoCache = svcp.GetRequiredService<RepositoryCache>();
            var dbContext = svcp.GetRequiredService<TDbContext>();

            return new EfCoreUnitOfWork(svcp, dbContext, repoCache);
        });

        return services;
    }

    private static RepositoryCache BuildRepositoryCache(
        IServiceCollection services
    )
    {
        var genericRepoType = typeof(IGenericRepository<,>);

        var concreteRepoTypes = services
            .SelectMany(x =>
            {
                var interfacesToCheck = x.ServiceType.GetInterfaces().Concat([x.ServiceType]);

                var repoInterfaces = interfacesToCheck
                    .Where(y => y.IsGenericType)
                    .Select(y => (repoInterface: y, genericInterface: y.GetGenericTypeDefinition()))
                    .Where(y => genericRepoType.IsAssignableFrom(y.genericInterface))
                    .Select(y => (ServiceType: x.ServiceType, AggregateType: y.repoInterface.GetGenericArguments()[0]))
                    .ToArray();

                return repoInterfaces;
            });

        var cache = new RepositoryCache();
        foreach (var repoType in concreteRepoTypes)
        {
            cache.Add(repoType.AggregateType, svcp => svcp.GetRequiredService(repoType.ServiceType));
        }

        return cache;
    }

    // used as unique singleton key
    // key is Aggregate type of the Repository
    private class RepositoryCache : Dictionary<Type, Func<IServiceProvider, object>> { }
}
