using POS.Domains.Customer.Abstractions.Carts;
using POS.Domains.Customer.Abstractions.Orders;
using POS.Domains.Customer.Abstractions.Orders.Events;
using POS.Domains.Customer.Domain.Carts;
using POS.Domains.Customer.Domain.Orders.Events;
using POS.Domains.Customer.Domain.Orders.Exceptions;
using POS.Domains.Customer.Domain.Orders.Models;
using POS.Shared.Domain;
using POS.Shared.Domain.Generic;
using POS.Shared.Domain.Generic.Mapper;

namespace POS.Domains.Customer.Domain.Orders;

/// <summary>
/// Aggregate root for customer orders
/// </summary>
public class Order : AggregateRoot
{
    private OrderState _state;

    /// <inheritdoc/>
    public override Guid Id => _state.Id;

    /// <summary>
    /// Date and time when the order was created.
    /// </summary>
    public DateTimeOffset CreatedAt => _state.CreatedAt;

    /// <summary>
    /// Date and time when the order was last changed at.
    /// </summary>
    public DateTimeOffset LastChanegdAt
    {
        get => _state.LastChangedAt;
        private set => _state.LastChangedAt = value;
    }

    /// <summary>
    /// State of the order
    /// </summary>
    public OrderStates State
    {
        get => _state.State;
        private set => _state.State = value;
    }

    /// <summary>
    /// Price summary of the order
    /// </summary>
    public OrderPriceSummary PriceSummary => _state.PriceSummary;

    /// <summary>
    /// Items of the order.
    /// </summary>
    public IReadOnlyList<OrderItem> OrderItems => _state.Items;

    /// <summary>
    /// Information about the payment.
    /// </summary>
    public PaymentInfo? PaymentInfo
    {
        get => _state.PaymentInfo;
        private set => _state.PaymentInfo = value;
    }

    /// <inheritdoc/>
    public override TState GetCurrentState<TState>() => (dynamic)_state;

    /// <summary>
    /// Creates a new order based on a cart.
    /// </summary>
    public static Order CreateByCartCheckout(
        Cart cart,
        DateTimeOffset createdAt
    )
    {
        var orderItems = cart.Items
            .Select(x => CreateOrderItem(x))
            .ToList();

        var order = new Order(new OrderState
        {
            Id = Guid.NewGuid(),
            CreatedAt = createdAt,
            LastChangedAt = createdAt,
            State = OrderStates.Created,
            Items = orderItems,
            PriceSummary = CalculateOrderPriceSummary(cart.Currency, orderItems)
        });

        order.Apply(new OrderCreatedByCheckoutEvent(
            order.Id,
            cart.Id,
            order.CreatedAt,
            order.State,
            order.PriceSummary,
            order.OrderItems
        ));

        return order;
    }

    /// <summary>
    /// Creates a new <see cref="Order"/>.
    /// </summary>
    public Order(
        OrderState state
    )
    {
        _state = state;
    }

    private static OrderPriceSummary CalculateOrderPriceSummary(
        string currency,
        IEnumerable<OrderItem> orderItems
    )
    {
        var totalItemPrice = GrossNetPrice.CreateZero(currency);
        var discount = GrossNetPrice.CreateZero(currency);
        var deliveryCosts = GrossNetPrice.CreateZero(currency);

        foreach (var orderItem in orderItems)
        {
            totalItemPrice += orderItem.TotalPrice.ToDomain();
        }

        var totalPrice = totalItemPrice + deliveryCosts - discount;
        return new OrderPriceSummary(
            totalItemPrice.ToDto(),
            totalPrice.ToDto(),
            deliveryCosts.ToDto(),
            discount.ToDto()
        );
    }

    private static OrderItem CreateOrderItem(CartItem cartItem)
    {
        var orderItem = new OrderItem(
            Guid.NewGuid(),
            cartItem.Id,
            cartItem.Name,
            cartItem.Description,
            cartItem.UnitPrice.ToDto(),
            cartItem.Quantity,
            (cartItem.UnitPrice.Price * cartItem.Quantity).ToDto()
        );
        return orderItem;
    }

    private void CheckAllowedStates(OrderStates expectedState)
    {
        if (State != expectedState) throw new OrderStateException(Id, State, expectedState);
    }

    #region PaymentRequest

    /// <summary>
    /// Checks if payment can be requested for this order.
    /// </summary>
    public void CheckPaymentCanBeRequested()
    {
        CheckAllowedStates(OrderStates.Created);
        if (PaymentInfo is null) return;
        if (PaymentInfo.PayedAt is null) return;

        throw new OrderAlreadyPaidException(Id);
    }

    /// <summary>
    /// The payment was successfully requested.
    /// </summary>
    public void PaymentRequested(
        Guid paymentId,
        DateTimeOffset requestedAt
    )
    {
        CheckPaymentCanBeRequested();

        PaymentInfo = new PaymentInfo
        {
            PaymentId = paymentId,
            RequestedAt = requestedAt
        };

        Apply(new OrderPaymentRequestedEvent(
            Id,
            requestedAt,
            PaymentInfo
        ));
    }

    #endregion
}
