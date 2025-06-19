using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.Domains.Customer.UseCases.Carts.CartUseCase;
using POS.Domains.Customer.UseCases.Carts.CartUseCase.Dtos;
using POS.Shared.Infrastructure.Api.Dtos;

namespace POS.Domains.Customer.Api.Controller;

/// <summary>
/// Responsible for handling carts based on menu items for customers
/// </summary>
[ApiVersion(1)]
[ApiController]
[Route("v1/[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartUseCase _cartService;

    /// <summary>
    /// Creates a new <see cref="CartController"/>
    /// </summary>
    /// <param name="cartService"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public CartController(ICartUseCase cartService)
    {
        _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
    }

    /// <summary>
    /// Create a new cart.
    /// </summary>
    /// <response code="201">The cart was successfully created.</response>
    /// <response code="400">The cart or it's content was incorrect.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/json")]
    public async Task<IActionResult> CreateCartAsync(CreateCartDto dto)
    {
        await _cartService.CreateCartAsync(dto);
        return Created();
    }

    /// <summary>
    /// Retrieve a specific cart by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier (UUID) of the cart.</param>
    /// <response code="404">The cart was not found.</response>
    [HttpGet("{id}")]
    [ProducesResponseType<CartDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/json")]
    public async Task<IActionResult> GetCartById(Guid id)
    {
        var cart = await _cartService.GetCartByIdAsync(id);
        return Ok(cart);
    }

    /// <summary>
    /// Add a new item to the cart.
    /// </summary>
    /// <response code="201">The item was successfully added.</response>
    /// <response code="400">The cart or the item was invalid.</response>
    [HttpPost("{id}/items")]
    [ProducesResponseType<CartItemDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/json")]
    public async Task<IActionResult> AddItemAsync(Guid id, AddItemDto dto)
    {
        var createdItem = await _cartService.AddItemToCartAsync(id, dto);
        return Ok(createdItem);
    }

    /// <summary>
    /// Update an existing item of the cart.
    /// </summary>
    /// <response code="200">The item was successfully updated.</response>
    /// <response code="400">The cart or the item was invalid.</response>
    [HttpPatch("{id}/items")]
    [ProducesResponseType<CartItemDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/json")]
    public async Task<IActionResult> UpdateItemAsync(Guid id, UpdateItemDto dto)
    {
        var updatedItemDto = await _cartService.UpdateCartItemAsync(id, dto);
        return Ok(updatedItemDto);
    }

    /// <summary>
    /// Retrieve all cart items of the cart.
    /// </summary>
    /// <param name="id">The unique identifier (UUID) of the cart.</param>
    /// <param name="token">The pagination token to retrieve.</param>
    /// <response code="404">The cart was not found.</response>
    [HttpGet("{id}/items")]
    [ProducesResponseType<ResultSetDto<CartItemDto>>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/json")]
    public async Task<IActionResult> GetCardItems(Guid id, string? token = null)
    {
        var resultset = await _cartService.GetCartItemsAsync(id, token);
        return Ok(resultset);
    }

    /// <summary>
    /// Checks out cart.
    /// </summary>
    /// <param name="id">The unique identifier (UUID) of the cart.</param>
    /// <param name="dto"></param>
    /// <response code="404">The cart was not found.</response>
    [HttpPost("{id}/checkout")]
    [ProducesResponseType<CartCheckedOutDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/json")]
    public async Task<IActionResult> Checkout(Guid id, [FromBody] CartCheckOutDto dto)
    {
        var result = await _cartService.CheckoutCartAsync(id, dto);
        return Ok(result);
    }
}
