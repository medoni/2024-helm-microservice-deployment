namespace POS.Domains.Payment.Service.Domain;
/// <summary>
/// Persistable state of the payment
/// </summary>
public class PaymentEntity
{
    /// <summary>
    /// Id of the payment.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// ID of the entity for which the payment should be requested.
    /// </summary>
    public required string EntityId { get; init; }

    /// <summary>
    /// Type of the entity for which the payment should be requested
    /// </summary>
    public required EntityTypes EntityType { get; init; }

    /// <summary>
    /// Date and time when the request was started.
    /// </summary>
    public required DateTimeOffset RequestedAt { get; init; }

    /// <summary>
    /// State of the payment
    /// </summary>
    public required PaymentStates State { get; init; }

    /// <summary>
    /// Payment provider.
    /// </summary>
    public required PaymentProviders Provider { get; init; }

    /// <summary>
    /// State of the payment provider.
    /// </summary>
    public required PaymentProviderState ProviderState { get; init; }

    /// <summary>
    /// Date and time when the request was successfully paid.
    /// </summary>
    public DateTimeOffset? PayedAt { get; init; }
}
