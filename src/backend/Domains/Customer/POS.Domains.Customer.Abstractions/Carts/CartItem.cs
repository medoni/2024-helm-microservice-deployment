using POS.Shared.Domain.Generic;
using System.Diagnostics.CodeAnalysis;

namespace POS.Domains.Customer.Abstractions.Carts;

/// <summary>
/// Cart item.
/// </summary>
public record CartItem
{
    /// <summary>
    /// Menu item id.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// Date and time when the cart item was created.
    /// </summary>
    public required DateTimeOffset CreatedAt { get; init; }

    /// <summary>
    /// Menu item id.
    /// </summary>
    public required Guid MenuItemId { get; init; }

    /// <summary>
    /// Name of the item.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Description of the item.
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// Unit price of the item.
    /// </summary>
    public required PriceInfo UnitPrice { get; init; }

    /// <summary>
    /// Date and time when the item was last changed at.
    /// </summary>
    public DateTimeOffset LastChangedAt { get; set; }

    /// <summary>
    /// Quantity of the cart item.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Creates a new <see cref="CartItem"/>.
    /// </summary>
    public CartItem() { }

    /// <summary>
    /// Creates a new <see cref="CartItem"/>.
    /// </summary>
    [SetsRequiredMembers]
    public CartItem(
        Guid id,
        DateTimeOffset createdAt,
        DateTimeOffset lastChangedAt,
        Guid menuItemId,
        string name,
        string description,
        PriceInfo price,
        int quantity
    )
    {
        Id = id;
        CreatedAt = createdAt;
        MenuItemId = menuItemId;
        Name = name;
        Description = description;
        UnitPrice = price;
        LastChangedAt = lastChangedAt;
        Quantity = quantity;
    }
}
