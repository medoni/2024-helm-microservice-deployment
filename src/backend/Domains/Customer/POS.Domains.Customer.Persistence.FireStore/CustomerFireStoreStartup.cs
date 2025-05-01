using Google.Cloud.Firestore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using POS.Domains.Customer.Domain.Carts;
using POS.Domains.Customer.Domain.Menus;
using POS.Domains.Customer.Domain.Orders;
using POS.Domains.Customer.Persistence.Carts;
using POS.Domains.Customer.Persistence.Carts.Decorators;
using POS.Domains.Customer.Persistence.FireStore.Configurations;
using POS.Domains.Customer.Persistence.FireStore.Repositories;
using POS.Domains.Customer.Persistence.FireStore.UnitOfWork;
using POS.Domains.Customer.Persistence.Menus;
using POS.Domains.Customer.Persistence.Menus.Decorators;
using POS.Domains.Customer.Persistence.Orders;
using POS.Domains.Customer.Persistence.Orders.Decorators;
using POS.Shared.Domain;
using POS.Shared.Persistence.Repositories;
using POS.Shared.Persistence.UOW;

namespace POS.Domains.Customer.Persistence.FireStore;

/// <summary>
/// Extension methods for configuring FireStore for Customer
/// </summary>
public static class CustomerFireStoreStartup
{
    /// <summary>
    /// Adds Customer support for FireStore to DI-Container.
    /// </summary>
    public static IServiceCollection AddCustomerFireStoreSupport(this IServiceCollection services)
    {
        services.AddFireStoreRepository<IMenuRespository, MenuRepository, Menu>((svcp, firestoreDb, options) =>
            new MenuRepository(firestoreDb, options.MenusCollectionName)
        );
        services.Decorate<IMenuRespository, LoggingMenuRepositoryDecorator>();

        services.AddFireStoreRepository<ICartRepository, CartRepository, Cart>((svcp, firestoreDb, options) =>
            new CartRepository(firestoreDb, options.CartsCollectionName)
        );
        services.Decorate<ICartRepository, LoggingCartRepositoryDecorator>();

        services.AddFireStoreRepository<IOrderRepository, OrderRepository, Order>((svcp, firestoreDb, options) =>
            new OrderRepository(firestoreDb, options.OrdersCollectionName)
        );
        services.Decorate<IOrderRepository, LoggingOrderRepositoryDecorator>();

        services.AddUnitOfWork<FireStoreUnitOfWork>();

        return services;
    }

    private static IServiceCollection AddFireStoreRepository<TRepositoryService, TRepositoryImpl, TAggregate>(
        this IServiceCollection services,
        Func<IServiceProvider, FirestoreDb, CustomerFireStoreSettings, TRepositoryImpl> factory
    )
    where TRepositoryService : class, IGenericRepository<TAggregate>
    where TRepositoryImpl : class, TRepositoryService
    where TAggregate : AggregateRoot
    {
        services.AddScoped<TRepositoryService, TRepositoryImpl>(svcp =>
        {
            var customerFireStoreOptions = svcp.GetRequiredService<IOptions<CustomerFireStoreSettings>>().Value;
            var firestoreDb = FirestoreDb.Create(customerFireStoreOptions.ProjectId);

            var repo = factory(svcp, firestoreDb, customerFireStoreOptions);
            return repo;
        });

        return services;
    }
}
