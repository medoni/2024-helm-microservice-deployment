using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using POS.Persistence.PostgreSql.Data;

namespace POS.Persistence.PostgreSql.HealthChecks;
internal class POSDbContextHealthCheck(POSDbContext dbContext) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            await Task.WhenAny(
                dbContext.Menus.FirstOrDefaultAsync(),
                Task.Factory.StartNew(async () =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(3));
                    throw new TimeoutException();
                })
            );

            return HealthCheckResult.Healthy("POS Database is healthy.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("POS Database is unhealthy.", ex);
        }
    }
}
