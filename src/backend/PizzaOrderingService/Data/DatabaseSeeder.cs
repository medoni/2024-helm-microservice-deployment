using Microsoft.EntityFrameworkCore;
using PizzaOrderingService.Domain;

namespace PizzaOrderingService.Data;

public class DatabaseSeeder
{
    public static async Task SeedDatabaseAsync(PizzaDbContext context)
    {
        if (!await context.Pizzas.AnyAsync())
        {
            context.Pizzas.AddRange(
                new Pizza(
                    Guid.NewGuid(),
                    "Margherita",
                    "Tomato, Mozzarella, Basil",
                    9.99m
                ),
                new Pizza(
                    Guid.NewGuid(),
                    "Pepperoni",
                    "Tomato, Mozzarella, Pepperoni",
                    12.99m
                ),
                new Pizza(
                    Guid.NewGuid(),
                    "Vegetarian",
                    "Tomato, Mozzarella, Vegetables",
                    11.99m
                )
            );

            await context.SaveChangesAsync();
        }
    }
}
