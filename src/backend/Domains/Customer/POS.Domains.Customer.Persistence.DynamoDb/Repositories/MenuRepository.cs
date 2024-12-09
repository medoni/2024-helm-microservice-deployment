using Amazon.DynamoDBv2.DataModel;
using POS.Domains.Customer.Domain.Menus;
using POS.Domains.Customer.Persistence.Menus;

namespace POS.Domains.Customer.Persistence.DynamoDb.Repositories;
internal class MenuRepository : BaseDynamoDbRepository<Menu, MenuState>, IMenuRespository
{
    public MenuRepository(
        IDynamoDBContext dynamoDBContext,
        DynamoDBOperationConfig dynamoDbOperationConfig
    ) : base(dynamoDBContext, dynamoDbOperationConfig)
    {
    }

    protected override Menu CreateAggregate(MenuState state)
    => new Menu(state);

    public Task<Menu?> GetActiveAsync()
    {
        throw new NotImplementedException();
    }
}
