using Microsoft.EntityFrameworkCore;

namespace PizzaOrderingService.Data;

internal static class ServiceCollectionExtensionMethods
{
    public static IServiceCollection AddPizzaDbContext(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<PizzaDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("PizzaDb")));
        return services;
    }
}
