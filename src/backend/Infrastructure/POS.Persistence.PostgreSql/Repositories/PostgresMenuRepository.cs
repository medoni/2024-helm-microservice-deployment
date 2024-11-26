using Microsoft.EntityFrameworkCore;
using POS.Domains.Customer.Domain.Menus;
using POS.Domains.Customer.Persistence.Menus;
using POS.Persistence.PostgreSql.Data;
using POS.Persistence.PostgreSql.Data.Customer;
using POS.Persistence.PostgreSql.Mapper.Customer;

namespace POS.Persistence.PostgreSql.Repositories;
internal partial class PostgresMenuRepository :
    PostgresAggregateRootRepository<Menu, MenuState, MenuEntity>,
    IMenuRespository
{

    public PostgresMenuRepository(POSDbContext dbContext)
    : base(dbContext)
    {
    }

    private IQueryable<MenuEntity> CreateQuery()
    {
        return DbContext.Menus
            .TagWithCallSite()
            .Include(x => x.Sections).ThenInclude(x => x.Items);
    }

    protected override IQueryable<MenuEntity> GetEntityByIdQuery(Guid id)
    => CreateQuery()
        .Where(x => x.Id == id);

    protected override IQueryable<MenuEntity> IterateEntitiesQuery()
    => CreateQuery()
        .OrderByDescending(x => x.ActivatedAt)
        .ThenByDescending(x => x.CreatedAt);

    protected override Menu CreateAggregateFromEntity(MenuEntity entity)
    {
        var state = entity.ToState();
        var aggregate = new Menu(state);
        return aggregate;
    }

    public async Task<Menu?> GetActiveAsync()
    {
        var result = await CreateQuery()
            .AsNoTracking()
            .Where(x => x.IsActive == true)
            .FirstOrDefaultAsync();

        if (result is null) return null;

        var aggregate = CreateAggregateFromEntity(result);
        return aggregate;
    }
}
