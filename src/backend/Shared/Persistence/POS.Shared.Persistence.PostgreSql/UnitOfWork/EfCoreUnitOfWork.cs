using Microsoft.EntityFrameworkCore;
using POS.Shared.Domain;
using POS.Shared.Persistence.Repositories;
using POS.Shared.Persistence.UOW;
using System.Collections.Concurrent;

namespace POS.Shared.Persistence.PostgreSql.UnitOfWork;
internal class EfCoreUnitOfWork : IUnitOfWork
{
    private readonly IServiceProvider _serviceProvider;
    private readonly DbContext _dbContext;
    private readonly IReadOnlyDictionary<Type, Func<IServiceProvider, object>> _repositoryFactory;
    private readonly ConcurrentDictionary<(Type, Guid), object> _trackedInstances;

    public EfCoreUnitOfWork(
        IServiceProvider serviceProvider,
        DbContext dbContext,
        IReadOnlyDictionary<Type, Func<IServiceProvider, object>> repositoryFactory
    )
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));

        _trackedInstances = new();
    }

    public void Add<TAggregate>(TAggregate aggregate) where TAggregate : AggregateRoot<Guid>
    {
        var cacheKey = (Type: typeof(TAggregate), Id: aggregate.Id);
        if (!_trackedInstances.TryAdd(cacheKey, aggregate)) throw new InvalidOperationException($"The aggregate of type '{cacheKey.Type}' and id '{cacheKey.Id}' has been already tracked.");

        var repo = GetRepository<TAggregate>();
        repo.AddAsync(aggregate);
    }

    public Task<TAggregate> GetAsync<TAggregate>(Guid id) where TAggregate : AggregateRoot<Guid>
    {
        var cacheKey = (typeof(TAggregate), id);
        var aggregate = (TAggregate)_trackedInstances.GetOrAdd(cacheKey, LoadAggregate);

        return Task.FromResult(aggregate);

        object LoadAggregate((Type type, Guid id) key)
        {
            var repo = GetRepository<TAggregate>();
            var aggregate = repo.GetByIdAsync(key.id)
                .ConfigureAwait(false).GetAwaiter().GetResult();

            return aggregate;
        }
    }

    public async Task CommitAsync()
    {
        foreach (var trackedItems in _trackedInstances)
        {
            var aggregate = (dynamic)trackedItems.Value;
            await CommitItemAsync(aggregate);
        }
        await _dbContext.SaveChangesAsync();
        _trackedInstances.Clear();
    }

    private async Task CommitItemAsync<TAggregate>(TAggregate item)
    where TAggregate : AggregateRoot<Guid>
    {
        var repo = GetRepository<TAggregate>();
        await repo.UpdateAsync(item);
    }

    private IGenericRepository<TAggregate, Guid> GetRepository<TAggregate>()
    where TAggregate : AggregateRoot<Guid>
    {
        var aggregateType = typeof(TAggregate);

        if (!_repositoryFactory.TryGetValue(aggregateType, out var repoFactory)) throw new InvalidOperationException($"No repository for aggregate '{aggregateType}' was found.");

        var repo = (IGenericRepository<TAggregate, Guid>)repoFactory(_serviceProvider);
        return repo;
    }
}
