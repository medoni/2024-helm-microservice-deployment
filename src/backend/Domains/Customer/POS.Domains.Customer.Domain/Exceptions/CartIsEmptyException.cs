namespace POS.Domains.Customer.Domain.Exceptions;

/// <summary>
/// Thrown when a cart is empty
/// </summary>
public class CartIsEmptyException : Exception
{
    /// <summary>
    /// Creates a new <see cref="CartIsEmptyException"/>
    /// </summary>
    public CartIsEmptyException(Guid cartId) : base($"The cart with id '{cartId}' is empty.")
    {

    }
}
