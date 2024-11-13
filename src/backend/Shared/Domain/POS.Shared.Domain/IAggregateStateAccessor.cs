namespace POS.Shared.Domain;

/// <summary>
/// Definition to access to state of an Aggregate
/// </summary>
public interface IAggregateStateAccessor
{
    /// <summary>
    /// Returns the state of an Aggregate
    /// </summary>
    public TState GetCurrentState<TState>();
}
