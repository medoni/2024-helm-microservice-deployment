namespace POS.Shared.Domain;

/// <summary>
/// Base definition for an AggregateRoot.
/// </summary>
/// <typeparam name="TID">Type of the ID to identify this AggregateRoot</typeparam>
public abstract class AggregateRoot<TID> : IAggregateStateAccessor
{
    /// <summary>
    /// The Id of the AggregateRoot
    /// </summary>
    public abstract TID Id
    {
        get;
    }

    /// <summary>
    /// Returns the current state of this AggregateRoot. The state can be used to store the underlaying data.
    /// </summary>
    public abstract TState GetCurrentState<TState>();
}
