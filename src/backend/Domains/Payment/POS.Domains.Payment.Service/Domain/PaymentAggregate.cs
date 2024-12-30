using POS.Domains.Payment.Service.Events;
using POS.Domains.Payment.Service.Services.PaymentProvider;
using POS.Shared.Domain;
using POS.Shared.Domain.Generic;
using POS.Shared.Domain.Generic.Mapper;

namespace POS.Domains.Payment.Service.Domain;

/// <summary>
///
/// </summary>
public class PaymentAggregate : AggregateRoot
{
    private PaymentAggregateState _state;

    /// <summary>
    /// Id of the payment
    /// </summary>
    public override Guid Id => _state.Id;

    /// <summary>
    /// Current state of the payment.
    /// </summary>
    public PaymentStates State
    {
        get => _state.State;
        private set => _state.State = value;
    }

    /// <summary>
    /// The used payment provider
    /// </summary>
    public PaymentProviderTypes PaymentProvider => _state.PaymentProvider;

    /// <summary>
    /// The entity type that should be payed.
    /// </summary>
    public EntityTypes EntityType => _state.EntityType;

    /// <summary>
    /// The id of the entity that should be payed.
    /// </summary>
    public string EntityId => _state.EntityId;

    /// <summary>
    /// Date and time when the payment was created.
    /// </summary>
    public DateTimeOffset CreatedAt => _state.CreatedAt;

    /// <summary>
    /// Description for the payment. This includes, for example, a summary of the entire order.
    /// </summary>
    public string Description => _state.Description;

    /// <summary>
    /// Total amount of the payment.
    /// </summary>
    public GrossNetPrice TotalAmount => _state.TotalAmount.ToDomain();

    /// <summary>
    /// Allowed links on the payment.
    /// </summary>
    public IReadOnlyList<PaymentLinkDescription> Links => _state.Links;

    /// <inheritdoc/>
    public override TState GetCurrentState<TState>()
    => (dynamic)_state;

    #region Create and ctor

    /// <summary>
    /// Creates a new payment in state
    /// </summary>
    /// <param name="paymentProvider">The payment provider that is used for the payment.</param>
    /// <param name="entityType">Type of the entity that should be payed.</param>
    /// <param name="entityId">Id of the entity that should be payed.</param>
    /// <param name="createAt">Date and time when payment is being created.</param>
    /// <param name="description">Description for the payment. This includes, for example, a summary of the entire order.</param>
    /// <param name="totalAmount">Total amount of the payment.</param>
    public static PaymentAggregate Create(
        PaymentProviderTypes paymentProvider,
        EntityTypes entityType,
        string entityId,
        DateTimeOffset createAt,
        string description,
        GrossNetPrice totalAmount
    )
    {
        var state = new PaymentAggregateState
        {
            Id = Guid.NewGuid(),
            State = PaymentStates.Created,
            PaymentProvider = paymentProvider,
            EntityType = entityType,
            EntityId = entityId,
            CreatedAt = createAt,
            Description = description,
            TotalAmount = totalAmount.ToDto()
        };

        var aggregate = new PaymentAggregate(state);
        aggregate.Apply(new PaymentCreatedEvent(
            aggregate.Id,
            aggregate.PaymentProvider,
            aggregate.EntityType,
            aggregate.EntityId,
            aggregate.CreatedAt,
            aggregate.Description,
            aggregate.TotalAmount.ToDto()
        ));

        return aggregate;
    }

    /// <summary>
    /// Creates a new <see cref="PaymentAggregate"/>
    /// </summary>
    public PaymentAggregate(PaymentAggregateState state)
    {
        _state = state ?? throw new ArgumentNullException(nameof(state));
    }

    #endregion
}
