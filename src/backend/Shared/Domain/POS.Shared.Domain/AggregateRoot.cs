using POS.Shared.Domain.Events;

namespace POS.Shared.Domain;

/// <summary>
/// Base definition for an AggregateRoot.
/// </summary>
public abstract class AggregateRoot : IAggregateStateAccessor
{
    /// <summary>
    /// The Id of the AggregateRoot
    /// </summary>
    public abstract Guid Id
    {
        get;
    }

    /// <summary>
    /// Returns the current state of this AggregateRoot. The state can be used to store the underlaying data.
    /// </summary>
    public abstract TState GetCurrentState<TState>();

    private readonly List<IDomainEvent> _changes = new();

    /// <summary>
    ///
    /// </summary>
    protected void Apply(IDomainEvent domainEvent)
    {
        lock (_changes)
        {
            _changes.Add(domainEvent);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public IDomainEvent[] GetUncommittedChanges()
    {
        lock (_changes)
        {
            return _changes.ToArray();
        }
    }

    /// <summary>
    /// Returns all uncommitted changes and clears aggregate of them.
    /// </summary>
    /// <returns>Array of new uncommitted events</returns>
    public IDomainEvent[] FlushUncommittedChanges()
    {
        lock (_changes)
        {
            var changes = _changes.ToArray();
            _changes.Clear();
            return changes;
        }
    }
}
