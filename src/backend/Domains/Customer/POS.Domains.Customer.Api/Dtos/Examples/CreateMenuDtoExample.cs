using POS.Domains.Customer.UseCases.CRUDMenuUseCase.Dtos;
using POS.Shared.Domain.Generic.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace POS.Domains.Customer.Api.Dtos.Examples;

/// <summary>
/// Responsible for creating example data for <see cref="CreateMenuDto"/>
/// </summary>
public class CreateMenuDtoExample : IExamplesProvider<CreateMenuDto>
{
    /// <summary>
    /// Returns example data for <see cref="CreateMenuDto"/>
    /// </summary>
    public CreateMenuDto GetExamples()
    => new CreateMenuDto(
        Guid.Parse("51e85e5a-bb24-454c-8797-188710b5ed84"),
        new List<MenuSectionDto>()
        {
            MenuSectionDto.Create(
                "Pizza",
                new List<MenuItemDto>()
                {
                    MenuItemDto.Create("Pizza Margherita", new MoneyDto(7.80m, "EUR"), "Classic pizza with tomatoes and cheese.", new[] { "Tomato", "Cheese" }),
                    MenuItemDto.Create("Pizza Pepperoni", new MoneyDto(9.50m, "EUR"), "Spicy pepperoni pizza with cheese.", new[] { "Tomato", "Cheese", "Pepperoni" }),
                    MenuItemDto.Create("Pizza Four Seasons", new MoneyDto(10.00m, "EUR"), "Pizza with mushrooms, ham, artichokes, and olives.", new[] { "Tomato", "Cheese", "Mushrooms", "Ham", "Artichokes", "Olives" }),
                    MenuItemDto.Create("Pizza Hawaiian", new MoneyDto(9.00m, "EUR"), "Ham and pineapple pizza.", new[] { "Tomato", "Cheese", "Ham", "Pineapple" }),
                    MenuItemDto.Create("Pizza Vegetariana", new MoneyDto(8.70m, "EUR"), "Vegetarian pizza with mixed vegetables.", new[] { "Tomato", "Cheese", "Bell Peppers", "Onions", "Mushrooms" })
                }
            ),
            MenuSectionDto.Create(
                "Pasta",
                new List<MenuItemDto>()
                {
                    MenuItemDto.Create("Spaghetti Bolognese", new MoneyDto(8.00m, "EUR"), "Spaghetti with rich meat sauce.", new[] { "Spaghetti", "Beef", "Tomato" }),
                    MenuItemDto.Create("Penne Arrabbiata", new MoneyDto(7.50m, "EUR"), "Penne pasta with spicy tomato sauce.", new[] { "Penne", "Tomato", "Chili" }),
                    MenuItemDto.Create("Lasagna", new MoneyDto(9.50m, "EUR"), "Layers of pasta with meat and cheese sauce.", new[] { "Pasta", "Beef", "Cheese" }),
                    MenuItemDto.Create("Fettuccine Alfredo", new MoneyDto(8.80m, "EUR"), "Fettuccine in creamy Alfredo sauce.", new[] { "Fettuccine", "Cream", "Cheese" }),
                    MenuItemDto.Create("Pasta Carbonara", new MoneyDto(8.70m, "EUR"), "Pasta with bacon, eggs, and cheese.", new[] { "Pasta", "Bacon", "Egg", "Cheese" })
                }
            ),
            MenuSectionDto.Create(
                "Appetizers",
                new List<MenuItemDto>()
                {
                    MenuItemDto.Create("Garlic Bread", new MoneyDto(3.00m, "EUR"), "Toasted bread with garlic and herbs.", new[] { "Bread", "Garlic" }),
                    MenuItemDto.Create("Bruschetta", new MoneyDto(4.20m, "EUR"), "Toasted bread with tomatoes and basil.", new[] { "Bread", "Tomato", "Basil" }),
                    MenuItemDto.Create("Mozzarella Sticks", new MoneyDto(4.80m, "EUR"), "Fried mozzarella sticks with marinara sauce.", new[] { "Mozzarella", "Breadcrumbs" }),
                    MenuItemDto.Create("Stuffed Mushrooms", new MoneyDto(5.50m, "EUR"), "Mushrooms stuffed with cheese and herbs.", new[] { "Mushrooms", "Cheese", "Herbs" }),
                    MenuItemDto.Create("Antipasto Plate", new MoneyDto(6.80m, "EUR"), "Assorted meats, cheeses, and olives.", new[] { "Ham", "Salami", "Cheese", "Olives" })
                }
            ),
            MenuSectionDto.Create(
                "Salads",
                new List<MenuItemDto>()
                {
                    MenuItemDto.Create("Caesar Salad", new MoneyDto(6.00m, "EUR"), "Classic Caesar with croutons and Parmesan.", new[] { "Lettuce", "Croutons", "Parmesan" }),
                    MenuItemDto.Create("Greek Salad", new MoneyDto(6.50m, "EUR"), "Salad with tomatoes, cucumbers, olives, and feta.", new[] { "Tomato", "Cucumber", "Olives", "Feta" }),
                    MenuItemDto.Create("Caprese Salad", new MoneyDto(5.80m, "EUR"), "Tomatoes, mozzarella, and basil drizzled with olive oil.", new[] { "Tomato", "Mozzarella", "Basil" }),
                    MenuItemDto.Create("Mixed Salad", new MoneyDto(5.00m, "EUR"), "A mix of fresh seasonal vegetables.", new[] { "Lettuce", "Tomato", "Cucumber", "Carrot" }),
                    MenuItemDto.Create("Arugula and Parmesan Salad", new MoneyDto(6.20m, "EUR"), "Arugula topped with shaved Parmesan.", new[] { "Arugula", "Parmesan" })
                }
            ),
            MenuSectionDto.Create(
                "Drinks",
                new List<MenuItemDto>()
                {
                    MenuItemDto.Create("Water", new MoneyDto(2.00m, "EUR"), "500ml bottled water.", new[] { "Water" }),
                    MenuItemDto.Create("Coca Cola", new MoneyDto(2.50m, "EUR"), "500ml Coca Cola.", new[] { "Soda" }),
                    MenuItemDto.Create("Lemonade", new MoneyDto(2.50m, "EUR"), "Refreshing lemonade.", new[] { "Lemon", "Sugar", "Water" }),
                    MenuItemDto.Create("Red Wine", new MoneyDto(4.50m, "EUR"), "Glass of house red wine.", new[] { "Wine" }),
                    MenuItemDto.Create("Beer", new MoneyDto(3.50m, "EUR"), "500ml of local beer.", new[] { "Beer" })
                }
            ),
            MenuSectionDto.Create(
                "Desserts",
                new List<MenuItemDto>()
                {
                    MenuItemDto.Create("Tiramisu", new MoneyDto(4.00m, "EUR"), "Classic Italian coffee-flavored dessert.", new[] { "Mascarpone", "Coffee", "Cocoa" }),
                    MenuItemDto.Create("Panna Cotta", new MoneyDto(3.80m, "EUR"), "Italian cream dessert with berries.", new[] { "Cream", "Berries" }),
                    MenuItemDto.Create("Gelato", new MoneyDto(3.00m, "EUR"), "Italian ice cream, assorted flavors.", new[] { "Ice Cream" }),
                    MenuItemDto.Create("Cannoli", new MoneyDto(4.50m, "EUR"), "Pastry shells filled with sweet ricotta.", new[] { "Ricotta", "Pastry" }),
                    MenuItemDto.Create("Chocolate Mousse", new MoneyDto(3.80m, "EUR"), "Rich chocolate mousse.", new[] { "Chocolate", "Cream" })
                }
            ),
            MenuSectionDto.Create(
                "Calzones",
                new List<MenuItemDto>()
                {
                    MenuItemDto.Create("Calzone Classico", new MoneyDto(8.50m, "EUR"), "Classic calzone with ham and cheese.", new[] { "Ham", "Cheese", "Tomato" }),
                    MenuItemDto.Create("Vegetable Calzone", new MoneyDto(8.20m, "EUR"), "Calzone stuffed with vegetables and cheese.", new[] { "Tomato", "Cheese", "Vegetables" }),
                    MenuItemDto.Create("Calzone Pepperoni", new MoneyDto(9.00m, "EUR"), "Calzone filled with pepperoni and cheese.", new[] { "Pepperoni", "Cheese", "Tomato" }),
                    MenuItemDto.Create("Calzone Spinach & Ricotta", new MoneyDto(8.70m, "EUR"), "Calzone with spinach and ricotta.", new[] { "Spinach", "Ricotta", "Cheese" }),
                    MenuItemDto.Create("Meat Lover's Calzone", new MoneyDto(9.50m, "EUR"), "Loaded with assorted meats.", new[] { "Ham", "Pepperoni", "Sausage", "Cheese" })
                }
            )
        }
    );
}
