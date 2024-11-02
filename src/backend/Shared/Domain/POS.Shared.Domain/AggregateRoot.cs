namespace POS.Shared.Domain;

public abstract class AggregateRoot<TID> : IAggregateStateAccessor
{
    public abstract TID Id
    {
        get;
    }

    public abstract TState GetCurrentState<TState>();
}
