using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.Domains.Customer.UseCases.Orders.OrderUseCase;
using POS.Domains.Customer.UseCases.Orders.OrderUseCase.Dtos;

namespace POS.Domains.Customer.Api.Controller;

/// <summary>
/// Responsible for handling orders for customers
/// </summary>
[ApiVersion(1)]
[ApiController]
[Route("v1/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderUseCase _orderService;

    /// <summary>
    /// Creates a new <see cref="CartController"/>
    /// </summary>
    /// <param name="orderService"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public OrderController(IOrderUseCase orderService)
    {
        _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
    }

    /// <summary>
    /// Retrieve a specific order by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier (UUID) of the order.</param>
    /// <response code="404">The order was not found.</response>
    [HttpGet("{id}")]
    [ProducesResponseType<OrderDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/json")]
    public async Task<IActionResult> GetOrderById(Guid id)
    {
        var cart = await _orderService.GetOrderByIdAsync(id);
        return Ok(cart);
    }
}
