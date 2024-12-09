using Amazon.DynamoDBv2.DataModel;
using POS.Domains.Customer.Domain.Carts;
using POS.Domains.Customer.Persistence.Carts;
using POS.Domains.Customer.Persistence.DynamoDb.Entities;
using System.Text.Json;

namespace POS.Domains.Customer.Persistence.DynamoDb.Repositories;
internal class CartRepository : BaseDynamoDbRepository<Cart, CartEntity>, ICartRepository
{
    public CartRepository(
        IDynamoDBContext dynamoDBContext,
        DynamoDBOperationConfig dynamoDbOperationConfig
    ) : base(dynamoDBContext, dynamoDbOperationConfig)
    {
    }

    protected override Cart CreateAggregate(CartEntity entity)
    {
        var state = JsonSerializer.Deserialize<CartState>(entity.Payload)!;
        var aggregate = new Cart(state);
        return aggregate;
    }

    protected override CartEntity CreateDynamoDbEntity(Cart aggregate)
    {
        var state = aggregate.GetCurrentState<CartState>();

        var entity = new CartEntity
        {
            Id = aggregate.Id.ToString(),
            CreatedAt = aggregate.CreatedAt.ToString("O"),
            Payload = JsonSerializer.Serialize(state)
        };
        return entity;
    }
}
