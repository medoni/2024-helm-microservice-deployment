using POS.Domains.Customer.Abstractions.Carts;

namespace POS.Domains.Customer.Domain.Carts;

/// <summary>
/// State to represent a cart
/// </summary>
/// <param name="Id">Id of the cart.</param>
/// <param name="CreatedAt">Date and time when the cart was created.</param>
/// <param name="MenuId">Active menu id when the cart was created.</param>
/// <param name="Currency">Currency of the cart.</param>
public record CartState
(
    Guid Id,
    DateTimeOffset CreatedAt,
    Guid MenuId,
    string Currency
)
{
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
    public CartState(
        Guid id,
        DateTimeOffset createdAt,
        Guid menuId,
        string currency,
        IList<CartItem> items
    ) : this(id, createdAt, menuId, currency)
    {
        Items = items ?? throw new ArgumentNullException(nameof(items));
    }
}
