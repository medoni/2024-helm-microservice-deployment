using POS.Shared.Domain.Generic.Dtos;
using System.Diagnostics.CodeAnalysis;

namespace POS.Domains.Customer.UseCases.Orders.OrderUseCase.Dtos;

/// <summary>
/// Dto for an order item
/// </summary>
public class OrderItemDto
{
    /// <summary>
    /// Item id
    /// </summary>
    public required Guid ItemId { get; set; }

    /// <summary>
    /// Cart id, from where the item was created.
    /// </summary>
    public required Guid? CartItemId { get; set; }

    /// <summary>
    /// Name of the item.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Description of the item.
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// Unit price of the item.
    /// </summary>
    public required PriceInfoDto UnitPrice { get; set; }

    /// <summary>
    /// Quantity of the item.
    /// </summary>
    public required int Quantity { get; set; }

    /// <summary>
    /// Total price of the item.
    /// </summary>
    public required GrossNetPriceDto TotalPrice { get; set; }

    /// <summary>
    /// Creates a new <see cref="OrderItemDto"/>.
    /// </summary>
    public OrderItemDto()
    {
    }

    /// <summary>
    /// Creates a new <see cref="OrderItemDto"/>.
    /// </summary>
    [SetsRequiredMembers]
    public OrderItemDto(
        Guid itemId,
        Guid? cartItemId,
        string name,
        string description,
        PriceInfoDto unitPrice,
        int quantity,
        GrossNetPriceDto totalPrice
    )
    {
        ItemId = itemId;
        CartItemId = cartItemId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        UnitPrice = unitPrice ?? throw new ArgumentNullException(nameof(unitPrice));
        Quantity = quantity;
        TotalPrice = totalPrice ?? throw new ArgumentNullException(nameof(totalPrice));
    }
}
