using POS.Domains.Customer.Abstractions.Orders;
using System.Diagnostics.CodeAnalysis;

namespace POS.Domains.Customer.UseCases.Orders.OrderUseCase.Dtos;

/// <summary>
/// Dto for use Order
/// </summary>
public class OrderDto
{
    /// <summary>
    /// Id of the order
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// Date and time when the order was created.
    /// </summary>
    public required DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Date and time when the order was last changed at.
    /// </summary>
    public required DateTimeOffset LastChangedAt { get; set; }

    /// <summary>
    /// State of the order.
    /// </summary>
    public required OrderStates State { get; set; }

    /// <summary>
    /// Items of the order.
    /// </summary>
    public required List<OrderItemDto> Items { get; set; }

    /// <summary>
    /// Price information about the order.
    /// </summary>
    public required OrderPriceSummaryDto PriceSummary { get; set; }

    /// <summary>
    /// Creates a new <see cref="OrderDto"/>.
    /// </summary>
    [SetsRequiredMembers]
    public OrderDto(
        Guid id,
        DateTimeOffset createdAt,
        DateTimeOffset lastChangedAt,
        OrderStates state,
        List<OrderItemDto> items,
        OrderPriceSummaryDto priceSummary
    )
    {
        Id = id;
        CreatedAt = createdAt;
        LastChangedAt = lastChangedAt;
        State = state;
        Items = items ?? throw new ArgumentNullException(nameof(items));
        PriceSummary = priceSummary ?? throw new ArgumentNullException(nameof(priceSummary));
    }

    /// <summary>
    /// Creates a new <see cref="OrderDto"/>.
    /// </summary>
    public OrderDto()
    {
    }
}
