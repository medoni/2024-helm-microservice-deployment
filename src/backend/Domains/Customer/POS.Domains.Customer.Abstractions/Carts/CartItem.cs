using POS.Shared.Domain.Generic;

namespace POS.Domains.Customer.Domain.Carts;

/// <summary>
/// Cart item.
/// </summary>
/// <param name="Id">Id of the cart.</param>
/// <param name="MenuItemId">Menu item id.</param>
/// <param name="CreatedAt">Date and time when the cart item was created.</param>
/// <param name="Name">Name of the item.</param>
/// <param name="Description">Description of the item.</param>
/// <param name="UnitPrice">Unit price of the item.</param>
public record CartItem
(
    Guid Id,
    DateTimeOffset CreatedAt,
    Guid MenuItemId,
    string Name,
    string Description,
    PriceInfo UnitPrice
)
{
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
    public CartItem(
        Guid id,
        DateTimeOffset createdAt,
        DateTimeOffset lastChangedAt,
        Guid menuItemId,
        string name,
        string description,
        PriceInfo price,
        int quantity
    ) : this(id, createdAt, menuItemId, name, description, price)
    {
        LastChangedAt = lastChangedAt;
        Quantity = quantity;
    }
}
