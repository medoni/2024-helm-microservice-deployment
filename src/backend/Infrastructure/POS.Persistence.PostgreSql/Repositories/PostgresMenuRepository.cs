using Microsoft.EntityFrameworkCore;
using POS.Domains.Customer.Domain.Menus;
using POS.Domains.Customer.Domain.Menus.States;
using POS.Domains.Customer.Persistence.Menus;
using POS.Persistence.PostgreSql.Data;
using POS.Persistence.PostgreSql.Data.Customer;
using POS.Persistence.PostgreSql.Mapper.Customer;
using POS.Shared.Domain.Exceptions;

namespace POS.Persistence.PostgreSql.Repositories;
internal class PostgresMenuRepository : IMenuRespository
{
    private readonly POSDbContext _dbContext;

    public PostgresMenuRepository(POSDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    private IQueryable<MenuEntity> CreateQuery()
    {
        return _dbContext.Menus
            .TagWithCallSite()
            .Include(x => x.Sections).ThenInclude(x => x.Items);
    }

    public async Task AddAsync(Menu aggregate)
    {
        var state = aggregate.GetCurrentState<MenuState>();
        await _dbContext.Menus.AddAsync(state.ToEntity());
    }

    public async Task<Menu?> GetActiveAsync()
    {
        var result = await CreateQuery()
            .Where(x => x.IsActive == true)
            .FirstOrDefaultAsync();

        if (result is null) return null;

        var aggregate = CreateAggregate(result);
        return aggregate;
    }

    public async Task<Menu> GetByIdAsync(Guid id)
    {
        var result = await CreateQuery()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        if (result is null) throw new AggregateNotFoundException(typeof(Menu), id);

        var aggregate = CreateAggregate(result);
        return aggregate;
    }

    public async IAsyncEnumerable<Menu> IterateAsync()
    {
        var query = CreateQuery()
            .OrderByDescending(x => x.ActivatedAt)
            .ThenByDescending(x => x.CreatedAt)
            .AsAsyncEnumerable();

        await foreach (var item in query)
        {
            var aggregate = CreateAggregate(item);
            yield return aggregate;
        }
    }

    public Task UpdateAsync(Menu aggregate)
    {
        var state = aggregate.GetCurrentState<MenuState>();
        var entity = state.ToEntity();

        var trackedEntry = _dbContext.ChangeTracker.Entries<MenuEntity>()
            .Where(x => x.Property(y => y.Id).CurrentValue == aggregate.Id)
            .FirstOrDefault();

        if (trackedEntry == null)
        {
            _dbContext.Update(entity);
        }
        else
        {
            trackedEntry.CurrentValues.SetValues(entity);
        }

        return Task.CompletedTask;
    }

    private static Menu CreateAggregate(MenuEntity entity)
    {
        var state = entity.ToState();

        var aggregate = new Menu(state);
        return aggregate;
    }
}
