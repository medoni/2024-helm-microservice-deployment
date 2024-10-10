using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzaOrderingService.Data;
using PizzaOrderingService.Domain;

namespace PizzaOrderingService.Api;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly PizzaDbContext _context;

    public OrdersController(PizzaDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetOrders()
    {
        var orders = _context.Orders.Include(o => o.Pizza).ToList();
        return Ok(orders);
    }

    [HttpPost]
    public IActionResult CreateOrder([FromBody] Order order)
    {
        var pizza = _context.Pizzas.Find(order.PizzaId);
        if (pizza == null) return BadRequest("Pizza not found.");

        order.TotalPrice = pizza.Price * order.Quantity;
        _context.Orders.Add(order);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetOrders), new { id = order.Id }, order);
    }
}
