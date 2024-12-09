using Amazon.DynamoDBv2.DataModel;
using POS.Domains.Customer.Domain.Orders;
using POS.Domains.Customer.Persistence.DynamoDb.Entities;
using POS.Domains.Customer.Persistence.Orders;
using System.Text.Json;

namespace POS.Domains.Customer.Persistence.DynamoDb.Repositories;
internal class OrderRepository : BaseDynamoDbRepository<Order, OrderEntity>, IOrderRepository
{
    public OrderRepository(
        IDynamoDBContext dynamoDBContext,
        DynamoDBOperationConfig dynamoDbOperationConfig
    ) : base(dynamoDBContext, dynamoDbOperationConfig)
    {
    }

    protected override Order CreateAggregate(OrderEntity entity)
    {
        var state = JsonSerializer.Deserialize<OrderState>(entity.Payload)!;
        var aggregate = new Order(state);
        return aggregate;
    }

    protected override OrderEntity CreateDynamoDbEntity(Order aggregate)
    {
        var state = aggregate.GetCurrentState<Order>();

        var entity = new OrderEntity
        {
            Id = aggregate.Id.ToString(),
            CreatedAt = aggregate.CreatedAt.ToString("O"),
            Payload = JsonSerializer.Serialize(state)
        };
        return entity;
    }
}
