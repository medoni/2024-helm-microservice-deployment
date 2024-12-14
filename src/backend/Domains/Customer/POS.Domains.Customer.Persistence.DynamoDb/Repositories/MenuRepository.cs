using Amazon.DynamoDBv2.DataModel;
using POS.Domains.Customer.Domain.Menus;
using POS.Domains.Customer.Persistence.DynamoDb.Entities;
using POS.Domains.Customer.Persistence.Menus;
using System.Text.Json;

namespace POS.Domains.Customer.Persistence.DynamoDb.Repositories;
internal class MenuRepository : BaseDynamoDbRepository<Menu, MenuEntity>, IMenuRespository
{
    public MenuRepository(
        IDynamoDBContext dynamoDBContext,
        DynamoDBOperationConfig dynamoDbOperationConfig
    ) : base(dynamoDBContext, dynamoDbOperationConfig)
    {
    }

    public async Task<Menu?> GetActiveAsync()
    {
        var query = DynamoDbCtx.QueryAsync<MenuEntity>(1, new DynamoDBOperationConfig()
        {
            IndexName = "active",
            OverrideTableName = DynamoDbOperationConfig.OverrideTableName
        });

        var resultSet = await query.GetNextSetAsync();
        var activeEntity = resultSet.FirstOrDefault();
        if (activeEntity is null) return null;

        var activeMenu = CreateAggregate(activeEntity);
        return activeMenu;
    }

    protected override Menu CreateAggregate(MenuEntity entity)
    {
        var state = JsonSerializer.Deserialize<MenuState>(entity.Payload)!;
        var aggregate = new Menu(state);
        return aggregate;
    }

    protected override MenuEntity CreateDynamoDbEntity(Menu aggregate)
    {
        var state = aggregate.GetCurrentState<MenuState>();

        var entity = new MenuEntity
        {
            Id = aggregate.Id.ToString(),
            CreatedAt = aggregate.CreatedAt.ToString("O"),
            Payload = JsonSerializer.Serialize(state),
            Active = state.IsActive ? 1 : 0
        };
        return entity;
    }
}
