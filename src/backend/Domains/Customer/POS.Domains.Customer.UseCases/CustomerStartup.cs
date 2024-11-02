using Microsoft.Extensions.DependencyInjection;
using POS.Domains.Customer.UseCases.CRUDMenuUseCase;
using POS.Domains.Customer.UseCases.PublishMenuUseCase;

namespace POS.Domains.Customer.UseCases;
public static class CustomerStartup
{
    public static IServiceCollection AddCustomerUseCases(this IServiceCollection services)
    {
        return services
            .AddTransient<ICRUDMenuUseCase, DefaultCRUDMenuUseCase>()
            .AddTransient<IPublishMenuUseCase, DefaultPublishMenuUseCase>()
        ;
    }
}
