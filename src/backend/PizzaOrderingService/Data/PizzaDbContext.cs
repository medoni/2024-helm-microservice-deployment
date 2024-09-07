using Microsoft.EntityFrameworkCore;
using PizzaOrderingService.Domain;

namespace PizzaOrderingService.Data;

public class PizzaDbContext : DbContext
{
    public PizzaDbContext(DbContextOptions<PizzaDbContext> options) : base(options) { }

    public DbSet<Pizza> Pizzas { get; set; }
    public DbSet<Order> Orders { get; set; }
}
