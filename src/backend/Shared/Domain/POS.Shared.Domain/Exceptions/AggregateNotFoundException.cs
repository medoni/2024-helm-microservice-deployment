namespace POS.Shared.Domain.Exceptions;

/// <summary>
/// Exception that is thrown when an Aggregate was not found.
/// </summary>
public sealed class AggregateNotFoundException : Exception
{
    /// <summary>
    /// Type of the Aggregate that was not found.
    /// </summary>
    public Type AggregateType { get; }

    /// <summary>
    /// Id of the Aggregate that was not found.
    /// </summary>
    public object Id { get; }

    /// <summary>
    /// Creates a new <see cref="AggregateNotFoundException"/>
    /// </summary>
    public AggregateNotFoundException(Type type, object id)
    : this(type, id, $"The aggregate with id '{id}' was not found.")
    {
    }

    /// <summary>
    /// Creates a new <see cref="AggregateNotFoundException"/>
    /// </summary>
    public AggregateNotFoundException(Type type, object id, string? message) : base(message)
    {
        AggregateType = type;
        Id = id;
    }
}
