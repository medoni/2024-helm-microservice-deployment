using POS.Domains.Customer.UseCases.Carts.CartUseCase.Dtos;
using POS.Shared.Infrastructure.Api.Dtos;

namespace POS.Domains.Customer.UseCases.Carts.CartUseCase;

/// <summary>
/// Definition to work with carts.
/// </summary>
public interface ICartUseCase
{
    /// <summary>
    /// Creates a new cart.
    /// </summary>
    Task CreateCartAsync(CreateCartDto dto);

    /// <summary>
    /// Returns a cart by its id.
    /// </summary>
    Task<CartDto> GetCartByIdAsync(Guid id);

    /// <summary>
    /// Add a item to the cart. If the item still exists, the quantity will be increased.
    /// </summary>
    /// <param name="id">Id of the cart.</param>
    /// <param name="dto">Dto to add.</param>
    Task AddItemToCartAsync(Guid id, AddItemDto dto);

    /// <summary>
    /// Returns all cart items of the cart.
    /// </summary>
    /// <param name="id">The card id.</param>
    /// <param name="token">Pagination token</param>
    Task<ResultSetDto<CartItemDto>> GetCartItemsAsync(Guid id, string? token = null);

    /// <summary>
    /// Updates an existing item of the cart. If the quantity is zero, the item will be removed from cart.
    /// </summary>
    /// <param name="id">Id of the cart.</param>
    /// <param name="dto">Dto to update.</param>
    Task UpdateCartItemAsync(Guid id, UpdateItemDto dto);

    /// <summary>
    /// Checkout a cart.
    /// </summary>
    Task<CartCheckedOutDto> CheckoutCartAsync(Guid cartId, CartCheckOutDto dto);
}
