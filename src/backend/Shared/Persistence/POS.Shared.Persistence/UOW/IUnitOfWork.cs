using POS.Shared.Domain;

namespace POS.Shared.Persistence.UOW;

/// <summary>
/// Definition of an Unit-Of-Work
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Retrieves an aggregate by its ID.
    /// If an instance is already active, it returns the existing instance.
    /// Otherwise, it loads the instance from the underlying repository.
    /// </summary>
    Task<TAggregate> GetAsync<TAggregate>(Guid id)
        where TAggregate : AggregateRoot<Guid>;

    /// <summary>
    /// Adds a new Aggregate to the unit of work.
    /// </summary>
    void Add<TAggregate>(TAggregate aggregate)
        where TAggregate : AggregateRoot<Guid>;

    /// <summary>
    /// Saves all changes made in the unit of work.
    /// </summary>
    Task CommitAsync();
}
