using POS.Domains.Customer.Abstractions.Carts;

namespace POS.Domains.Customer.UseCases.Carts.CartUseCase.Dtos;

/// <summary>
/// Dto with informations about the cart checkout.
/// </summary>
public class CartCheckoutInfoDto
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
    public CartCheckoutInfoDto(
        DateTimeOffset checkedOutAt,
        Guid orderId
    )
    {
        CheckedOutAt = checkedOutAt;
        OrderId = orderId;
    }
}
