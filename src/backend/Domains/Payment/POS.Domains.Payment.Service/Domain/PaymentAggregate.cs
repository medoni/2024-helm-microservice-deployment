using POS.Domains.Payment.Service.Domain.Events;
using POS.Domains.Payment.Service.Domain.Exceptions;
using POS.Domains.Payment.Service.Domain.Models;
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
    /// Date and time when the payment was requested.
    /// </summary>
    public DateTimeOffset? RequestedAt
    {
        get => _state.RequestedAt;
        private set => _state.RequestedAt = value;
    }

    /// <summary>
    /// Total amount of the payment.
    /// </summary>
    public GrossNetPrice TotalAmount
    {
        get => _state.TotalAmount.ToDomain();
        private set => _state.TotalAmount = value.ToDto();
    }

    /// <summary>
    /// Internal payload of the payment provider.
    /// </summary>
    public string? PaymentProviderPayload
    {
        get => _state.PaymentProviderPayload;
        private set => _state.PaymentProviderPayload = value;
    }

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

    private void CheckAllowedStates(PaymentStates expectedState)
    {
        if (State != expectedState) throw new PaymentStateException(Id, State, expectedState);
    }

    #region Request payment

    /// <summary>
    /// Checks if a payment can be requested.
    /// </summary>
    public void CheckPaymentCanBeRequested()
    {
        CheckAllowedStates(PaymentStates.Created);
    }

    /// <summary>
    /// The payment was successfully requested by the payment provider.
    /// </summary>
    /// <param name="totalAmountRequested">The total amount that was requested.</param>
    /// <param name="requestedAt">Date and time when the payment was requested.</param>
    /// <param name="paymentApprovalLink">The link to approve the payment by the buyer.</param>
    /// <param name="paymentProviderState">Internal state object of the underlaying payment provider.</param>
    public void PaymentRequested(
        GrossNetPrice totalAmountRequested,
        DateTimeOffset requestedAt,
        PaymentLinkDescription paymentApprovalLink,
        string paymentProviderState
    )
    {
        CheckPaymentCanBeRequested();

        State = PaymentStates.Requested;
        TotalAmount = totalAmountRequested;
        RequestedAt = requestedAt;

        _state.Links.RemoveAll(x => x.Type == PaymentLinkTypes.Approve);
        _state.Links.Add(paymentApprovalLink);
        _state.PaymentProviderPayload = paymentProviderState;

        Apply(new PaymentRequestedEvent(
            Id,
            PaymentProvider,
            EntityType,
            EntityId,
            requestedAt,
            totalAmountRequested.ToDto(),
            Links.ToList(),
            paymentProviderState
        ));
    }

    #endregion
}
