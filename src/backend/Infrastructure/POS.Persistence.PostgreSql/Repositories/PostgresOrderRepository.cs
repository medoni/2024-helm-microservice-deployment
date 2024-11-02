using Microsoft.EntityFrameworkCore;
using POS.Domains.Customer.Domain.Orders;
using POS.Domains.Customer.Persistence.Orders;
using POS.Persistence.PostgreSql.Data;
using POS.Persistence.PostgreSql.Data.Customer;
using POS.Persistence.PostgreSql.Mapper.Customer;

namespace POS.Persistence.PostgreSql.Repositories;

internal partial class PostgresOrderRepository :
    PostgresAggregateRootRepository<Order, OrderState, OrderEntity>,
    IOrderRepository
{
    public PostgresOrderRepository(POSDbContext dbContext) : base(dbContext)
    {
    }

    private IQueryable<OrderEntity> CreateQuery()
    {
        return DbContext.Orders
            .TagWithCallSite()
            .Include(x => x.Items)
            .Include(x => x.PriceSummary);
    }

    protected override IQueryable<OrderEntity> GetEntityByIdQuery(Guid id)
    {
        return CreateQuery()
            .Where(x => x.Id == id);
    }

    protected override IQueryable<OrderEntity> IterateEntitiesQuery()
    {
        return CreateQuery()
            .OrderBy(x => x.CreatedAt);
    }

    protected override Order CreateAggregateFromEntity(OrderEntity entity)
    {
        var state = entity.ToState();
        var aggregate = new Order(state);
        return aggregate;
    }
}
