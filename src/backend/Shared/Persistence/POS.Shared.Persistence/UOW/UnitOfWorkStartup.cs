using Microsoft.Extensions.DependencyInjection;
using POS.Shared.Persistence.Repositories;

namespace POS.Shared.Persistence.UOW;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/> regarding Unit-Of-Work implementation.
/// /// </summary>
public static class UnitOfWorkStartup
{
    /// <summary>
    /// Adds Unit-Of-Work support to the <see cref="IServiceCollection"/>
    /// </summary>
    public static IServiceCollection AddUnitOfWork<TUnitOfWork>(
        this IServiceCollection services
    )
    where TUnitOfWork : class, IUnitOfWork
    {
        services.AddTransient<UnitOfWorkFactory>(svcp =>
        {
            return () => svcp.GetRequiredService<IUnitOfWork>();
        });

        services.AddSingleton<BaseRepositoryFactory>(svcp => BuildRepositoryFactory(services));

        services.AddTransient<IUnitOfWork, TUnitOfWork>();

        return services;
    }

    private static BaseRepositoryFactory BuildRepositoryFactory(
        IServiceCollection services
    )
    {
        var genericRepoType = typeof(IGenericRepository<>);

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

        var cache = new BaseRepositoryFactory();
        foreach (var repoType in concreteRepoTypes)
        {
            cache[repoType.AggregateType] = svcp => svcp.GetRequiredService(repoType.ServiceType);
        }

        return cache;
    }
}
