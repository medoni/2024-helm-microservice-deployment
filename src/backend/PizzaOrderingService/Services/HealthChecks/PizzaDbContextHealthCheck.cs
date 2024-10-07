using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using PizzaOrderingService.Data;

namespace PizzaOrderingService.Services.HealthChecks
{
    public class PizzaDbContextHealthCheck(PizzaDbContext pizzaDb) : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default
        )
        {
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));

            try
            {
                await pizzaDb.Pizzas.FirstOrDefaultAsync(cts.Token);

                return HealthCheckResult.Healthy("PizzaDbContext is healthy.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("PizzaDbContext is unhealthy.", ex);
            }
        }
    }
}
