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
    private readonly ConcurrentDictionary<(Type, Guid), TrackedRecord> _trackedInstances;

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

    public void Add<TAggregate>(TAggregate aggregate) where TAggregate : AggregateRoot
    {
        var cacheKey = (Type: typeof(TAggregate), Id: aggregate.Id);
        var trackedItem = TrackedRecord.CreateAdd(aggregate, GetRepository<TAggregate>);

        if (!_trackedInstances.TryAdd(cacheKey, trackedItem)) throw new InvalidOperationException($"The aggregate of type '{cacheKey.Type}' and id '{cacheKey.Id}' has been already tracked.");
    }

    public Task<TAggregate> GetAsync<TAggregate>(Guid id) where TAggregate : AggregateRoot
    {
        var cacheKey = (typeof(TAggregate), id);
        var trackedItem = _trackedInstances.GetOrAdd(cacheKey, LoadAggregate);
        var aggregate = (TAggregate)trackedItem.Aggregate;

        return Task.FromResult(aggregate);

        TrackedRecord LoadAggregate((Type type, Guid id) key)
        {
            var repo = GetRepository<TAggregate>();
            var aggregate = repo.GetByIdAsync(key.id)
                .ConfigureAwait(false).GetAwaiter().GetResult();
            var trackedItem = TrackedRecord.CreateUpdate(aggregate, GetRepository<TAggregate>);

            return trackedItem;
        }
    }

    public async Task CommitAsync()
    {
        foreach (var trackedItems in _trackedInstances)
        {
            var trackedItem = trackedItems.Value;
            await trackedItem.CommitAction();
        }
        await _dbContext.SaveChangesAsync();
        _trackedInstances.Clear();
    }

    private IGenericRepository<TAggregate> GetRepository<TAggregate>()
    where TAggregate : AggregateRoot
    {
        var aggregateType = typeof(TAggregate);

        if (!_repositoryFactory.TryGetValue(aggregateType, out var repoFactory)) throw new InvalidOperationException($"No repository for aggregate '{aggregateType}' was found.");

        var repo = (IGenericRepository<TAggregate>)repoFactory(_serviceProvider);
        return repo;
    }

    private record TrackedRecord(
        object Aggregate,
        Func<Task> CommitAction
    )
    {
        public static TrackedRecord CreateAdd<TAggregate>(
            TAggregate aggregate,
            Func<IGenericRepository<TAggregate>> getRepo
        )
        where TAggregate : AggregateRoot
        {
            var commitAction = async () =>
            {
                var repo = getRepo();
                await repo.AddAsync(aggregate);
            };

            return new TrackedRecord(
                aggregate,
                commitAction
            );
        }

        public static TrackedRecord CreateUpdate<TAggregate>(
            TAggregate aggregate,
            Func<IGenericRepository<TAggregate>> getRepo
        )
        where TAggregate : AggregateRoot
        {
            var commitAction = async () =>
            {
                var repo = getRepo();
                await repo.UpdateAsync(aggregate);
            };

            return new TrackedRecord(
                aggregate,
                commitAction
            );
        }
    }
}
