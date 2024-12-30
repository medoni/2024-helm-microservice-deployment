using POS.Domains.Payment.Service.Domain;

namespace POS.Domains.Payment.Service.Events;

/// <summary>
/// Abstract event, used when the state of the payment was changed.
/// </summary>
public abstract class PaymentStateChangedEvent
{
    /// <summary>
    /// Id of the payment
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// Current state of the payment.
    /// </summary>
    public required PaymentStates NewState { get; init; }

    /// <summary>
    /// Date and time when the payment was changed.
    /// </summary>
    public required DateTimeOffset OccurredAt { get; init; }

    /// <summary>
    /// Creates a new <see cref="PaymentStateChangedEvent"/>
    /// </summary>
    protected PaymentStateChangedEvent(
        Guid id,
        PaymentStates newState,
        DateTimeOffset occurredAt
    )
    {
        Id = id;
        NewState = newState;
        OccurredAt = occurredAt;
    }
}
