using Microsoft.Extensions.DependencyInjection;
using POS.Domains.Customer.UseCases.Carts.CartUseCase;
using POS.Domains.Customer.UseCases.Menus.CRUDMenuUseCase;
using POS.Domains.Customer.UseCases.Menus.PublishMenuUseCase;
using POS.Domains.Customer.UseCases.Orders.OrderUseCase;

namespace POS.Domains.Customer.UseCases;

/// <summary>
/// Responsible for adding use cases regarding to Customers.
/// </summary>
public static class CustomerStartup
{
    /// <summary>
    /// Adds use cases regarding to Customers to the Service Collection.
    /// </summary>
    public static IServiceCollection AddCustomerUseCases(this IServiceCollection services)
    {
        return services
            .AddTransient<ICRUDMenuUseCase, DefaultCRUDMenuUseCase>()
            .AddTransient<IPublishMenuUseCase, DefaultPublishMenuUseCase>()
            .AddTransient<IOrderUseCase, DefaultOrderUseCase>()
            .AddTransient<ICartUseCase, DefaultCartUseCase>()
        ;
    }
}
