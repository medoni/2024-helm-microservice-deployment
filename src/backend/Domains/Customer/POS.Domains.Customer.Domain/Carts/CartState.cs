using POS.Domains.Customer.Abstractions.Carts;
using System.Diagnostics.CodeAnalysis;

namespace POS.Domains.Customer.Domain.Carts;

/// <summary>
/// State to represent a cart
/// </summary>
public record CartState
{
    /// <summary>
    /// Id of the cart.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// Date and time when the cart was created.
    /// </summary>
    public required DateTimeOffset CreatedAt { get; init; }

    /// <summary>
    /// Active menu id when the cart was created.
    /// </summary>
    public required Guid MenuId { get; init; }

    /// <summary>
    /// Currency of the cart.
    /// </summary>
    public required string Currency { get; init; }

    /// <summary>
    /// State of the cart.
    /// </summary>
    public required CartStates State { get; set; }

    /// <summary>
    /// Date and time when the cart was last changed at.
    /// </summary>
    public required DateTimeOffset LastChangedAt { get; set; }

    /// <summary>
    /// Items of the cart.
    /// </summary>
    public IList<CartItem> Items { get; set; } = new List<CartItem>();

    /// <summary>
    /// Checkout info.
    /// </summary>
    public CartCheckoutInfo? CheckoutInfo { get; set; }

    /// <summary>
    /// Creates a new <see cref="CartState"/>.
    /// </summary>
    public CartState() { }

    /// <summary>
    /// Creates a new <see cref="CartState"/>.
    /// </summary>
    [SetsRequiredMembers]
    public CartState(
        Guid id,
        DateTimeOffset createdAt,
        Guid menuId,
        string currency,
        IList<CartItem> items
    )
    {
        Id = id;
        CreatedAt = createdAt;
        MenuId = menuId;
        Currency = currency;
        Items = items ?? throw new ArgumentNullException(nameof(items));
    }
}
