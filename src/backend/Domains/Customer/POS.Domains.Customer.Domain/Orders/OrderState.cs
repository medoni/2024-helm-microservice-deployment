using POS.Domains.Customer.Abstractions.Orders;
using POS.Domains.Customer.Domain.Orders.Models;

namespace POS.Domains.Customer.Domain.Orders;

/// <summary>
/// State for an Order
/// </summary>
public record OrderState
{
    /// <summary>
    /// Id of the order.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// Date and time when the order was created.
    /// </summary>
    public required DateTimeOffset CreatedAt { get; init; }

    /// <summary>
    /// Items of the order.
    /// </summary>
    public required IReadOnlyList<OrderItem> Items { get; init; }

    /// <summary>
    /// Price summary of the order.
    /// </summary>
    public required OrderPriceSummary PriceSummary { get; init; }

    /// <summary>
    /// State of the order.
    /// </summary>
    public required OrderStates State { get; set; }

    /// <summary>
    /// Date and time when the order was last changed at.
    /// </summary>
    public required DateTimeOffset LastChangedAt { get; set; }

    /// <summary>
    /// Information about the payment.
    /// </summary>
    public PaymentInfo? PaymentInfo { get; set; }
}
