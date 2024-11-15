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
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));

        try
        {
            await dbContext.Menus.FirstOrDefaultAsync(cts.Token);

            return HealthCheckResult.Healthy("POS Database is healthy.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("POS Database is unhealthy.", ex);
        }
    }
}
