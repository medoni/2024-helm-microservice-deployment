using Microsoft.EntityFrameworkCore;
using POS.Domains.Customer.Domain.Menus;
using POS.Domains.Customer.Domain.Menus.Dtos;
using POS.Domains.Customer.Persistence.Menus;
using POS.Persistence.PostgreSql.Data;
using POS.Shared.Domain.Exceptions;

namespace POS.Persistence.PostgreSql.Repositories;
internal class PostgresMenuRepository : IMenuRespository
{
    private readonly POSDbContext _dbContext;

    public PostgresMenuRepository(POSDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task AddAsync(Menu aggregate)
    {
        var entity = aggregate.GetCurrentState<MenuEntity>();
        await _dbContext.AddAsync(entity);
    }

    public async Task<Menu?> GetActiveAsync()
    {
        var result = await _dbContext.Menus
            .Where(x => x.IsActive == true)
            .FirstOrDefaultAsync();

        if (result is null) return null;

        var aggregate = CreateAggregate(result);
        return aggregate;
    }

    public async Task<Menu> GetByIdAsync(Guid id)
    {
        var result = await _dbContext.Menus.FindAsync(id);
        if (result is null) throw new AggregateNotFoundException(typeof(Menu), id);

        var aggregate = CreateAggregate(result);
        return aggregate;
    }

    public async IAsyncEnumerable<Menu> IterateAsync()
    {
        var query = _dbContext.Menus
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
        var entity = aggregate.GetCurrentState<MenuEntity>();
        _dbContext.Update(entity);

        return Task.CompletedTask;
    }

    private static Menu CreateAggregate(MenuEntity entity)
    {
        var aggregate = new Menu(entity);
        return aggregate;
    }
}
