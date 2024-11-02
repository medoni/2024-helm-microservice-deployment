namespace POS.Shared.Domain.Exceptions;
public class AggregateNotFoundException : Exception
{
    public Type AggregateType { get; }
    public object Id { get; }

    public AggregateNotFoundException(Type type, object id)
    : this(type, id, $"The aggregate with id '{id}' was not found.")
    {
    }

    public AggregateNotFoundException(Type type, object id, string? message) : base(message)
    {
        AggregateType = type;
        Id = id;
    }
}
