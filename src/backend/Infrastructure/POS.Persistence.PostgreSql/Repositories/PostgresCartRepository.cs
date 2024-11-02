using Microsoft.EntityFrameworkCore;
using POS.Domains.Customer.Domain.Carts;
using POS.Domains.Customer.Persistence.Carts;
using POS.Persistence.PostgreSql.Data;
using POS.Persistence.PostgreSql.Data.Customer;
using POS.Persistence.PostgreSql.Mapper.Customer;

namespace POS.Persistence.PostgreSql.Repositories;

internal partial class PostgresCartRepository :
    PostgresAggregateRootRepository<Cart, CartState, CartEntity>,
    ICartRepository
{
    public PostgresCartRepository(POSDbContext dbContext) : base(dbContext)
    {
    }

    private IQueryable<CartEntity> CreateQuery()
    {
        return DbContext.Carts
            .TagWithCallSite()
            .Include(x => x.Items)
            .Include(x => x.CheckoutInfo);
    }

    protected override IQueryable<CartEntity> GetEntityByIdQuery(Guid id)
    => CreateQuery()
        .Where(x => x.Id == id);

    protected override IQueryable<CartEntity> IterateEntitiesQuery()
    => CreateQuery()
        .OrderBy(x => x.CreatedAt);

    protected override Cart CreateAggregateFromEntity(CartEntity entity)
    {
        var state = entity.ToState();
        var aggregate = new Cart(state);
        return aggregate;
    }
}
