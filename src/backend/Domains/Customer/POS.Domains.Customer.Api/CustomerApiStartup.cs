using Microsoft.Extensions.DependencyInjection;
using POS.Domains.Customer.UseCases;
using Swashbuckle.AspNetCore.Filters;

namespace POS.Domains.Customer.Api;

/// <summary>
/// Responsible for adding Customer-Api to the entire application
/// </summary>
public static class CustomerApiStartup
{
    /// <summary>
    /// Adds Customer-Api support to the <see cref="IMvcBuilder"/> for the ASP.NET application.
    /// </summary>
    public static IMvcBuilder AddCustomerApi(this IMvcBuilder builder)
    {
        var assembly = typeof(CustomerApiStartup).Assembly;

        builder.AddApplicationPart(assembly);

        builder.Services.AddCustomerUseCases();
        builder.Services.AddSwaggerExamplesFromAssemblies(assembly);

        return builder;
    }
}
