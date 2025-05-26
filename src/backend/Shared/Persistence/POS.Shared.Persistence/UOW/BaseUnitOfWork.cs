using Microsoft.Extensions.DependencyInjection;
using POS.Shared.Domain;
using POS.Shared.Domain.Events;
using POS.Shared.Infrastructure.PubSub.Abstractions;
using POS.Shared.Persistence.Repositories;
using System.Collections.Concurrent;

namespace POS.Shared.Persistence.UOW;

/// <summary>
/// Base implementation of Unit-Of-Work pattern with specific Repositories per Aggregate type.
/// </summary>
public abstract class BaseUnitOfWork : IUnitOfWork
{
    private readonly IServiceProvider _serviceProvider;
    private readonly BaseRepositoryFactory _repositoryFactory;
    private readonly ConcurrentDictionary<(Type, Guid), TrackedRecord> _trackedInstances;
    private readonly IEventPublisher? _eventPublisher;

    /// <summary>
    /// Creates a new <see cref="BaseUnitOfWork"/>
    /// </summary>
    public BaseUnitOfWork(
        IServiceProvider serviceProvider,
        BaseRepositoryFactory repositoryFactory
    )
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));

        _trackedInstances = new();
        _eventPublisher = serviceProvider.GetService<IEventPublisher>();
    }

    /// <inheritdoc/>
    public void Add<TAggregate>(TAggregate aggregate) where TAggregate : AggregateRoot
    {
        var cacheKey = (Type: typeof(TAggregate), Id: aggregate.Id);
        var trackedItem = CreateAddRecord(aggregate, GetRepository<TAggregate>);

        if (!_trackedInstances.TryAdd(cacheKey, trackedItem)) throw new InvalidOperationException($"The aggregate of type '{cacheKey.Type}' and id '{cacheKey.Id}' has been already tracked.");
    }

    /// <summary>
    /// Creates a tracked record for adding the aggregate when committing the changes.
    /// </summary>
    protected abstract TrackedRecord CreateAddRecord<TAggregate>(
        TAggregate aggregate,
        Func<IGenericRepository<TAggregate>> getRepo
    )
    where TAggregate : AggregateRoot;

    /// <inheritdoc/>
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
            var trackedItem = CreateUpdateRecord(aggregate, GetRepository<TAggregate>);

            return trackedItem;
        }
    }

    /// <summary>
    /// Creates a tracked record for updating the aggregate when committing the changes.
    /// </summary>
    protected abstract TrackedRecord CreateUpdateRecord<TAggregate>(
        TAggregate aggregate,
        Func<IGenericRepository<TAggregate>> getRepo
    )
    where TAggregate : AggregateRoot;

    /// <inheritdoc/>
    public async Task CommitAsync()
    {
        var uncommittedEvents = _eventPublisher is null ? null : new List<IDomainEvent>();

        foreach (var trackedItems in _trackedInstances)
        {
            var trackedItem = trackedItems.Value;
            if (uncommittedEvents != null) uncommittedEvents.AddRange(trackedItem.Aggregate.GetUncommittedChanges());

            await trackedItem.CommitAction();
        }
        await FlushCommitsAsync();

        if (uncommittedEvents != null) await _eventPublisher!.PublishAsync(uncommittedEvents);
        _trackedInstances.Clear();
    }

    /// <summary>
    /// Flushes (saves) the outstanding changes to the underlaying database system.
    /// </summary>
    protected abstract Task FlushCommitsAsync();

    private IGenericRepository<TAggregate> GetRepository<TAggregate>()
    where TAggregate : AggregateRoot
    {
        var aggregateType = typeof(TAggregate);

        if (!_repositoryFactory.TryGetValue(aggregateType, out var repoFactory)) throw new InvalidOperationException($"No repository for aggregate '{aggregateType}' was found.");

        var repo = (IGenericRepository<TAggregate>)repoFactory(_serviceProvider);
        return repo;
    }

    /// <summary>
    /// Data class that stores Aggregate and corresponding commit function
    /// </summary>
    protected record TrackedRecord(
        AggregateRoot Aggregate,
        Func<Task> CommitAction
    )
    {
    }
}
