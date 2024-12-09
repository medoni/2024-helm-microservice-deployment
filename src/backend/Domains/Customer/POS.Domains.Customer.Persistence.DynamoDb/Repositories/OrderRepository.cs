using Amazon.DynamoDBv2.DataModel;
using POS.Domains.Customer.Domain.Orders;
using POS.Domains.Customer.Persistence.Orders;

namespace POS.Domains.Customer.Persistence.DynamoDb.Repositories;
internal class OrderRepository : BaseDynamoDbRepository<Order, OrderState>, IOrderRepository
{
    public OrderRepository(
        IDynamoDBContext dynamoDBContext,
        DynamoDBOperationConfig dynamoDbOperationConfig
    ) : base(dynamoDBContext, dynamoDbOperationConfig)
    {
    }

    protected override Order CreateAggregate(OrderState state)
    => new Order(state);
}
