using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Reflection;

namespace PizzaOrderingService.Services.HealthChecks;

public class VersionInfoHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default
    )
    {
        var assembly = Assembly
            .GetEntryAssembly();

        var informationalInfo = assembly?.GetCustomAttributes<AssemblyInformationalVersionAttribute>().FirstOrDefault();

        var data = new Dictionary<string, object>();
        data.Add("Version", informationalInfo?.InformationalVersion ?? "<NULL>");

        return Task.FromResult(
            HealthCheckResult.Healthy($"{assembly?.GetName().Name} is healthy.", data)
        );
    }
}
