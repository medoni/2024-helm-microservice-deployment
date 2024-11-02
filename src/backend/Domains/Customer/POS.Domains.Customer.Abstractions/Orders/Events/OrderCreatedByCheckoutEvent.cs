using POS.Shared.Domain.Events;

namespace POS.Domains.Customer.Abstractions.Orders.Events;

/// <summary>
/// Event raised when a new order was created by checking out a cart
/// </summary>
public record OrderCreatedByCheckoutEvent
(
    Guid OrderId,
    Guid CartId,
    DateTimeOffset CreatedAt,
    OrderStates State,
    OrderPriceSummary PriceSummary,
    IReadOnlyList<OrderItem> Items
) : IDomainEvent
{
}
