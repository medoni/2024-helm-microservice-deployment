namespace POS.Domains.Customer.Abstractions.Carts;

/// <summary>
/// Information when a cart was checkout
/// </summary>
public class CartCheckoutInfo
{
    /// <summary>
    /// Date and time when the cart was checked out.
    /// </summary>
    public DateTimeOffset CheckedOutAt { get; }

    /// <summary>
    /// Created order id.
    /// </summary>
    public Guid OrderId { get; }

    /// <summary>
    /// Creates a new <see cref="CartCheckoutInfo"/>.
    /// </summary>
    public CartCheckoutInfo(
        DateTimeOffset checkedOutAt,
        Guid orderId
    )
    {
        CheckedOutAt = checkedOutAt;
        OrderId = orderId;
    }
}
