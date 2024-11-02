using POS.Domains.Customer.UseCases.CRUDMenuUseCase.Dtos;
using POS.Shared.Domain.Generic.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace POS.Domains.Customer.Api.Dtos.Examples;

public class CreateMenuDtoExample : IExamplesProvider<CreateMenuDto>
{
    public CreateMenuDto GetExamples()
    => new CreateMenuDto(
        Guid.Parse("51e85e5a-bb24-454c-8797-188710b5ed84"),
        new List<MenuSectionDto>()
        {
            new MenuSectionDto(
                "Pizza",
                new List<MenuItemDto>()
                {
                    new MenuItemDto("Pizza Margherita", new MoneyDto(7.80m, "EUR"), "Classic pizza with tomatoes and cheese.", new[] { "Tomato", "Cheese" }),
                    new MenuItemDto("Pizza Pepperoni", new MoneyDto(9.50m, "EUR"), "Spicy pepperoni pizza with cheese.", new[] { "Tomato", "Cheese", "Pepperoni" }),
                    new MenuItemDto("Pizza Four Seasons", new MoneyDto(10.00m, "EUR"), "Pizza with mushrooms, ham, artichokes, and olives.", new[] { "Tomato", "Cheese", "Mushrooms", "Ham", "Artichokes", "Olives" }),
                    new MenuItemDto("Pizza Hawaiian", new MoneyDto(9.00m, "EUR"), "Ham and pineapple pizza.", new[] { "Tomato", "Cheese", "Ham", "Pineapple" }),
                    new MenuItemDto("Pizza Vegetariana", new MoneyDto(8.70m, "EUR"), "Vegetarian pizza with mixed vegetables.", new[] { "Tomato", "Cheese", "Bell Peppers", "Onions", "Mushrooms" })
                }
            ),
            new MenuSectionDto(
                "Pasta",
                new List<MenuItemDto>()
                {
                    new MenuItemDto("Spaghetti Bolognese", new MoneyDto(8.00m, "EUR"), "Spaghetti with rich meat sauce.", new[] { "Spaghetti", "Beef", "Tomato" }),
                    new MenuItemDto("Penne Arrabbiata", new MoneyDto(7.50m, "EUR"), "Penne pasta with spicy tomato sauce.", new[] { "Penne", "Tomato", "Chili" }),
                    new MenuItemDto("Lasagna", new MoneyDto(9.50m, "EUR"), "Layers of pasta with meat and cheese sauce.", new[] { "Pasta", "Beef", "Cheese" }),
                    new MenuItemDto("Fettuccine Alfredo", new MoneyDto(8.80m, "EUR"), "Fettuccine in creamy Alfredo sauce.", new[] { "Fettuccine", "Cream", "Cheese" }),
                    new MenuItemDto("Pasta Carbonara", new MoneyDto(8.70m, "EUR"), "Pasta with bacon, eggs, and cheese.", new[] { "Pasta", "Bacon", "Egg", "Cheese" })
                }
            ),
            new MenuSectionDto(
                "Appetizers",
                new List<MenuItemDto>()
                {
                    new MenuItemDto("Garlic Bread", new MoneyDto(3.00m, "EUR"), "Toasted bread with garlic and herbs.", new[] { "Bread", "Garlic" }),
                    new MenuItemDto("Bruschetta", new MoneyDto(4.20m, "EUR"), "Toasted bread with tomatoes and basil.", new[] { "Bread", "Tomato", "Basil" }),
                    new MenuItemDto("Mozzarella Sticks", new MoneyDto(4.80m, "EUR"), "Fried mozzarella sticks with marinara sauce.", new[] { "Mozzarella", "Breadcrumbs" }),
                    new MenuItemDto("Stuffed Mushrooms", new MoneyDto(5.50m, "EUR"), "Mushrooms stuffed with cheese and herbs.", new[] { "Mushrooms", "Cheese", "Herbs" }),
                    new MenuItemDto("Antipasto Plate", new MoneyDto(6.80m, "EUR"), "Assorted meats, cheeses, and olives.", new[] { "Ham", "Salami", "Cheese", "Olives" })
                }
            ),
            new MenuSectionDto(
                "Salads",
                new List<MenuItemDto>()
                {
                    new MenuItemDto("Caesar Salad", new MoneyDto(6.00m, "EUR"), "Classic Caesar with croutons and Parmesan.", new[] { "Lettuce", "Croutons", "Parmesan" }),
                    new MenuItemDto("Greek Salad", new MoneyDto(6.50m, "EUR"), "Salad with tomatoes, cucumbers, olives, and feta.", new[] { "Tomato", "Cucumber", "Olives", "Feta" }),
                    new MenuItemDto("Caprese Salad", new MoneyDto(5.80m, "EUR"), "Tomatoes, mozzarella, and basil drizzled with olive oil.", new[] { "Tomato", "Mozzarella", "Basil" }),
                    new MenuItemDto("Mixed Salad", new MoneyDto(5.00m, "EUR"), "A mix of fresh seasonal vegetables.", new[] { "Lettuce", "Tomato", "Cucumber", "Carrot" }),
                    new MenuItemDto("Arugula and Parmesan Salad", new MoneyDto(6.20m, "EUR"), "Arugula topped with shaved Parmesan.", new[] { "Arugula", "Parmesan" })
                }
            ),
            new MenuSectionDto(
                "Drinks",
                new List<MenuItemDto>()
                {
                    new MenuItemDto("Water", new MoneyDto(2.00m, "EUR"), "500ml bottled water.", new[] { "Water" }),
                    new MenuItemDto("Coca Cola", new MoneyDto(2.50m, "EUR"), "500ml Coca Cola.", new[] { "Soda" }),
                    new MenuItemDto("Lemonade", new MoneyDto(2.50m, "EUR"), "Refreshing lemonade.", new[] { "Lemon", "Sugar", "Water" }),
                    new MenuItemDto("Red Wine", new MoneyDto(4.50m, "EUR"), "Glass of house red wine.", new[] { "Wine" }),
                    new MenuItemDto("Beer", new MoneyDto(3.50m, "EUR"), "500ml of local beer.", new[] { "Beer" })
                }
            ),
            new MenuSectionDto(
                "Desserts",
                new List<MenuItemDto>()
                {
                    new MenuItemDto("Tiramisu", new MoneyDto(4.00m, "EUR"), "Classic Italian coffee-flavored dessert.", new[] { "Mascarpone", "Coffee", "Cocoa" }),
                    new MenuItemDto("Panna Cotta", new MoneyDto(3.80m, "EUR"), "Italian cream dessert with berries.", new[] { "Cream", "Berries" }),
                    new MenuItemDto("Gelato", new MoneyDto(3.00m, "EUR"), "Italian ice cream, assorted flavors.", new[] { "Ice Cream" }),
                    new MenuItemDto("Cannoli", new MoneyDto(4.50m, "EUR"), "Pastry shells filled with sweet ricotta.", new[] { "Ricotta", "Pastry" }),
                    new MenuItemDto("Chocolate Mousse", new MoneyDto(3.80m, "EUR"), "Rich chocolate mousse.", new[] { "Chocolate", "Cream" })
                }
            ),
            new MenuSectionDto(
                "Calzones",
                new List<MenuItemDto>()
                {
                    new MenuItemDto("Calzone Classico", new MoneyDto(8.50m, "EUR"), "Classic calzone with ham and cheese.", new[] { "Ham", "Cheese", "Tomato" }),
                    new MenuItemDto("Vegetable Calzone", new MoneyDto(8.20m, "EUR"), "Calzone stuffed with vegetables and cheese.", new[] { "Tomato", "Cheese", "Vegetables" }),
                    new MenuItemDto("Calzone Pepperoni", new MoneyDto(9.00m, "EUR"), "Calzone filled with pepperoni and cheese.", new[] { "Pepperoni", "Cheese", "Tomato" }),
                    new MenuItemDto("Calzone Spinach & Ricotta", new MoneyDto(8.70m, "EUR"), "Calzone with spinach and ricotta.", new[] { "Spinach", "Ricotta", "Cheese" }),
                    new MenuItemDto("Meat Lover's Calzone", new MoneyDto(9.50m, "EUR"), "Loaded with assorted meats.", new[] { "Ham", "Pepperoni", "Sausage", "Cheese" })
                }
            )
        }
    );
}
