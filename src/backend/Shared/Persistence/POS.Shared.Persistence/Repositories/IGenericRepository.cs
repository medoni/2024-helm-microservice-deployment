using POS.Shared.Domain;

namespace POS.Shared.Persistence.Repositories;

/// <summary>
/// Definition for an Repository that handles <see cref="AggregateRoot{TID}"/>s.
/// </summary>
/// <typeparam name="TAggregate">Type of the Aggregate.</typeparam>
/// <typeparam name="TID">Type of the Id of the Aggregate.</typeparam>
public interface IGenericRepository<TAggregate, TID>
where TAggregate : AggregateRoot<TID>
{
    /// <summary>
    /// Adds a new Aggregate to the repository.
    /// </summary>
    Task AddAsync(TAggregate aggregate);

    /// <summary>
    /// Gets an Aggregate by it's id.
    /// </summary>
    Task<TAggregate> GetByIdAsync(TID id);

    /// <summary>
    /// Updates an existing Aggregate.
    /// </summary>
    Task UpdateAsync(TAggregate aggregate);

    /// <summary>
    /// Iterates over stored Aggregates.
    /// </summary>
    IAsyncEnumerable<TAggregate> IterateAsync();
}
