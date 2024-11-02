using POS.Shared.Domain;

namespace POS.Shared.Persistence.Repositories;

/// <summary>
/// Definition for an Repository that handles <see cref="AggregateRoot"/>s.
/// </summary>
/// <typeparam name="TAggregate">Type of the Aggregate.</typeparam>
public interface IGenericRepository<TAggregate>
where TAggregate : AggregateRoot
{
    /// <summary>
    /// Adds a new Aggregate to the repository.
    /// </summary>
    Task AddAsync(TAggregate aggregate);

    /// <summary>
    /// Gets an Aggregate by it's id.
    /// </summary>
    Task<TAggregate> GetByIdAsync(Guid id);

    /// <summary>
    /// Updates an existing Aggregate.
    /// </summary>
    Task UpdateAsync(TAggregate aggregate);

    /// <summary>
    /// Iterates over stored Aggregates.
    /// </summary>
    IAsyncEnumerable<TAggregate> IterateAsync();
}
