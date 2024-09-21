using Microsoft.AspNetCore.Mvc;
using PizzaOrderingService.Data;
using PizzaOrderingService.Domain;

namespace PizzaOrderingService.Api;

[ApiController]
[Route("[controller]")]
public class PizzaController : ControllerBase
{
    private readonly PizzaDbContext _context;

    public PizzaController(PizzaDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetPizzas()
    {
        return Ok(_context.Pizzas.ToList());
    }

    [HttpPost]
    public IActionResult CreatePizza([FromBody] Pizza pizza)
    {
        _context.Pizzas.Add(pizza);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetPizzas), new { id = pizza.Id }, pizza);
    }
}
