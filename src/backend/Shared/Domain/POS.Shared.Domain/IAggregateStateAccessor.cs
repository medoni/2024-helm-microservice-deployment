namespace POS.Shared.Domain;
public interface IAggregateStateAccessor
{
    public TState GetCurrentState<TState>();
}
