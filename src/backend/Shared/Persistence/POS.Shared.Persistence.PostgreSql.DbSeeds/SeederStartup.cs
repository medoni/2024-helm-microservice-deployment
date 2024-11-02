using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace POS.Shared.Persistence.PostgreSql.DbSeeds;
public static class SeederStartup
{
    public static IServiceCollection AddSeederFromAssembly(
        this IServiceCollection services,
        Assembly assembly
    )
    {
        var seederDefinition = typeof(ISeeder);
        var seederTypes = assembly.DefinedTypes
            .Where(x => seederDefinition.IsAssignableFrom(x));

        foreach (var seederType in seederTypes)
        {
            services.AddTransient(seederDefinition, seederType);
        }

        return services;
    }

    public static IServiceCollection AddSeederSupport<TDbContext>(this IServiceCollection services)
    where TDbContext : DbContext
    {
        var dbContextAccessor = new Func<IServiceProvider, DbContext>(svcp =>
        {
            return svcp.GetRequiredService<TDbContext>();
        });

        services
            .AddSingleton<ISeederProcessor, PostgreSqlSeederProcessor>(svcp =>
            {
                return new PostgreSqlSeederProcessor(
                    svcp.GetRequiredService<ILogger<PostgreSqlSeederProcessor>>(),
                    dbContextAccessor
                );
            });

        return services;
    }
}
