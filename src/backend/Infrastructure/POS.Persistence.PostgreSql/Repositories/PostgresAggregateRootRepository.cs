using Microsoft.EntityFrameworkCore;
using POS.Domains.Customer.Domain.Menus;
using POS.Persistence.PostgreSql.Abstractions;
using POS.Persistence.PostgreSql.Data;
using POS.Shared.Domain;
using POS.Shared.Domain.Events;
using POS.Shared.Domain.Exceptions;
using POS.Shared.Persistence.Repositories;

namespace POS.Persistence.PostgreSql.Repositories;

internal abstract class PostgresAggregateRootRepository<TAggregate, TState, TEntity> : IGenericRepository<TAggregate>
where TAggregate : AggregateRoot
where TState : notnull
where TEntity : class, IEntity<Guid>
{
    protected readonly POSDbContext DbContext;

    protected PostgresAggregateRootRepository(
        POSDbContext dbContext
    )
    {
        DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext)); ;
    }

    protected abstract TAggregate CreateAggregateFromEntity(TEntity entity);

    public async Task AddAsync(TAggregate aggregate)
    {
        await ProcessUncommitedEventsAsync(aggregate.GetUncommittedChanges());
    }

    protected abstract IQueryable<TEntity> GetEntityByIdQuery(Guid id);
    public async Task<TAggregate> GetByIdAsync(Guid id)
    {
        var result = await GetEntityByIdQuery(id)
            .FirstOrDefaultAsync();

        if (result is null) throw new AggregateNotFoundException(typeof(Menu), id);

        var aggregate = CreateAggregateFromEntity(result);
        return aggregate;
    }

    protected abstract IQueryable<TEntity> IterateEntitiesQuery();
    public async IAsyncEnumerable<TAggregate> IterateAsync()
    {
        var query = IterateEntitiesQuery()
            .AsAsyncEnumerable();

        await foreach (var item in query)
        {
            var aggregate = CreateAggregateFromEntity(item);
            yield return aggregate;
        }
    }

    public async Task UpdateAsync(TAggregate aggregate)
    {
        await ProcessUncommitedEventsAsync(aggregate.GetUncommittedChanges());
    }

    private async Task ProcessUncommitedEventsAsync(IEnumerable<IDomainEvent> events)
    {
        foreach (var evt in events)
        {
            await ((dynamic)this).ProcessUncommitedEventAsync((dynamic)evt);
        }
    }
}
