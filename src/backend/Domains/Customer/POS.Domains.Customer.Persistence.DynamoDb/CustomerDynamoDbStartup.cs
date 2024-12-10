using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using POS.Domains.Customer.Domain.Carts;
using POS.Domains.Customer.Domain.Menus;
using POS.Domains.Customer.Domain.Orders;
using POS.Domains.Customer.Persistence.Carts;
using POS.Domains.Customer.Persistence.Carts.Decorators;
using POS.Domains.Customer.Persistence.DynamoDb.Configurations;
using POS.Domains.Customer.Persistence.DynamoDb.Repositories;
using POS.Domains.Customer.Persistence.DynamoDb.UnitOfWork;
using POS.Domains.Customer.Persistence.Menus;
using POS.Domains.Customer.Persistence.Menus.Decorators;
using POS.Domains.Customer.Persistence.Orders;
using POS.Domains.Customer.Persistence.Orders.Decorators;
using POS.Shared.Domain;
using POS.Shared.Persistence.Repositories;
using POS.Shared.Persistence.UOW;

namespace POS.Domains.Customer.Persistence.DynamoDb;

/// <summary>
///
/// </summary>
public static class CustomerDynamoDbStartup
{
    /// <summary>
    ///
    /// </summary>
    public static IServiceCollection AddCustomerDynamoDbSupport(this IServiceCollection services)
    {
        services.AddDynamoDbRepository<IMenuRespository, MenuRepository, Menu>((svcp, ddbCtx, options) =>
            new MenuRepository(ddbCtx, svcp.BuildOperationConfig(options.MenusTableName))
        );
        services.Decorate<IMenuRespository, LoggingMenuRepositoryDecorator>();

        services.AddDynamoDbRepository<ICartRepository, CartRepository, Cart>((svcp, ddbCtx, options) =>
            new CartRepository(ddbCtx, svcp.BuildOperationConfig(options.CartsTableName))
        );
        services.Decorate<ICartRepository, LoggingCartRepositoryDecorator>();

        services.AddDynamoDbRepository<IOrderRepository, OrderRepository, Order>((svcp, ddbCtx, options) =>
            new OrderRepository(ddbCtx, svcp.BuildOperationConfig(options.OrdersTableName))
        );
        services.Decorate<IOrderRepository, LoggingOrderRepositoryDecorator>();

        services.AddUnitOfWork<DynamoDbUnitOfWork>();

        return services;
    }

    private static IServiceCollection AddDynamoDbRepository<TRepositoryService, TRepositoryImpl, TAggregate>(
        this IServiceCollection services,
        Func<IServiceProvider, DynamoDBContext, CustomerDynamoDbOptions, TRepositoryImpl> factory
    )
    where TRepositoryService : class, IGenericRepository<TAggregate>
    where TRepositoryImpl : class, TRepositoryService
    where TAggregate : AggregateRoot
    {

        services.AddScoped<TRepositoryService, TRepositoryImpl>(svcp =>
        {
            var customerDbOptions = svcp.GetRequiredService<IOptions<CustomerDynamoDbOptions>>().Value;
            var clientConfig = new AmazonDynamoDBConfig();
            clientConfig.RegionEndpoint = RegionEndpoint.GetBySystemName(customerDbOptions.Region);
            var client = new AmazonDynamoDBClient(clientConfig);
            var ddbCtx = new DynamoDBContext(client);

            var repo = factory(svcp, ddbCtx, customerDbOptions);
            return repo;
        });

        return services;
    }

    private static DynamoDBOperationConfig BuildOperationConfig(this IServiceProvider svcp, string tableName)
    {
        return new DynamoDBOperationConfig()
        {
            OverrideTableName = tableName
        };
    }
}
