using Amazon.DynamoDBv2.DataModel;
using POS.Domains.Customer.Domain.Carts;
using POS.Domains.Customer.Persistence.Carts;

namespace POS.Domains.Customer.Persistence.DynamoDb.Repositories;
internal class CartRepository : BaseDynamoDbRepository<Cart, CartState>, ICartRepository
{
    public CartRepository(
        IDynamoDBContext dynamoDBContext,
        DynamoDBOperationConfig dynamoDbOperationConfig
    ) : base(dynamoDBContext, dynamoDbOperationConfig)
    {
    }

    protected override Cart CreateAggregate(CartState state)
    => new Cart(state);
}
