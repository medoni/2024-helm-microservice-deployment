using POS.Domains.Customer.Abstractions.Orders;

namespace POS.Domains.Customer.Domain.Orders;

/// <summary>
/// State for an Order
/// </summary>
public record OrderState
(
    Guid Id,
    DateTimeOffset CreatedAt,
    IReadOnlyList<OrderItem> Items,
    OrderPriceSummary PriceSummary
)
{
    /// <summary>
    /// State of the order.
    /// </summary>
    public required OrderStates State { get; set; }

    /// <summary>
    /// Date and time when the order was last changed at.
    /// </summary>
    public required DateTimeOffset LastChangedAt { get; set; }
}
