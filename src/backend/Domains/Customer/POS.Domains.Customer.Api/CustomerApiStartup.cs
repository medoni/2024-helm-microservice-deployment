using Microsoft.Extensions.DependencyInjection;
using POS.Domains.Customer.UseCases;
using Swashbuckle.AspNetCore.Filters;

namespace POS.Domains.Customer.Api;
public static class CustomerApiStartup
{
    public static IMvcBuilder AddCustomerApi(this IMvcBuilder builder)
    {
        var assembly = typeof(CustomerApiStartup).Assembly;

        builder.AddApplicationPart(assembly);

        builder.Services.AddCustomerUseCases();
        builder.Services.AddSwaggerExamplesFromAssemblies(assembly);

        return builder;
    }
}
