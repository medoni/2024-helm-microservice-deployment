using POS.Domains.Customer.UseCases.Menus.CRUDMenuUseCase.Dtos;
using POS.Shared.Domain.Generic;
using POS.Shared.Domain.Generic.Mapper;
using Swashbuckle.AspNetCore.Filters;

namespace POS.Domains.Customer.Api.Dtos.Examples;

/// <summary>
/// Responsible for creating example data for <see cref="CreateMenuDto"/>
/// </summary>
public class CreateMenuDtoExample : IExamplesProvider<CreateMenuDto>
{
    private const string Currency = "EUR";
    private const decimal RegularyVat = 7;

    /// <summary>
    /// Returns example data for <see cref="CreateMenuDto"/>
    /// </summary>
    public CreateMenuDto GetExamples()
    => new CreateMenuDto(
        Guid.Parse("51e85e5a-bb24-454c-8797-188710b5ed84"),
        "EUR",
        new List<MenuSectionDto>()
        {
            MenuSectionDto.Create(
                "Pizza",
                new List<MenuItemDto>()
                {
                    MenuItemDto.Create("Pizza Margherita", PriceInfo.CreateByGross(7.80m, RegularyVat, Currency).ToDto(), "Classic pizza with tomatoes and cheese.", new[] { "Tomato", "Cheese" }),
                    MenuItemDto.Create("Pizza Pepperoni", PriceInfo.CreateByGross(9.50m, RegularyVat, Currency).ToDto(), "Spicy pepperoni pizza with cheese.", new[] { "Tomato", "Cheese", "Pepperoni" }),
                    MenuItemDto.Create("Pizza Four Seasons", PriceInfo.CreateByGross(10.00m, RegularyVat, Currency).ToDto(), "Pizza with mushrooms, ham, artichokes, and olives.", new[] { "Tomato", "Cheese", "Mushrooms", "Ham", "Artichokes", "Olives" }),
                    MenuItemDto.Create("Pizza Hawaiian", PriceInfo.CreateByGross(9.00m, RegularyVat, Currency).ToDto(), "Ham and pineapple pizza.", new[] { "Tomato", "Cheese", "Ham", "Pineapple" }),
                    MenuItemDto.Create("Pizza Vegetariana", PriceInfo.CreateByGross(8.70m, RegularyVat, Currency).ToDto(), "Vegetarian pizza with mixed vegetables.", new[] { "Tomato", "Cheese", "Bell Peppers", "Onions", "Mushrooms" })
                }
            ),
            MenuSectionDto.Create(
                "Pasta",
                new List<MenuItemDto>()
                {
                    MenuItemDto.Create("Spaghetti Bolognese", PriceInfo.CreateByGross(8.00m, RegularyVat, Currency).ToDto(), "Spaghetti with rich meat sauce.", new[] { "Spaghetti", "Beef", "Tomato" }),
                    MenuItemDto.Create("Penne Arrabbiata", PriceInfo.CreateByGross(7.50m, RegularyVat, Currency).ToDto(), "Penne pasta with spicy tomato sauce.", new[] { "Penne", "Tomato", "Chili" }),
                    MenuItemDto.Create("Lasagna", PriceInfo.CreateByGross(9.50m, RegularyVat, Currency).ToDto(), "Layers of pasta with meat and cheese sauce.", new[] { "Pasta", "Beef", "Cheese" }),
                    MenuItemDto.Create("Fettuccine Alfredo", PriceInfo.CreateByGross(8.80m, RegularyVat, Currency).ToDto(), "Fettuccine in creamy Alfredo sauce.", new[] { "Fettuccine", "Cream", "Cheese" }),
                    MenuItemDto.Create("Pasta Carbonara", PriceInfo.CreateByGross(8.70m, RegularyVat, Currency).ToDto(), "Pasta with bacon, eggs, and cheese.", new[] { "Pasta", "Bacon", "Egg", "Cheese" })
                }
            ),
            MenuSectionDto.Create(
                "Appetizers",
                new List<MenuItemDto>()
                {
                    MenuItemDto.Create("Garlic Bread", PriceInfo.CreateByGross(3.00m, RegularyVat, Currency).ToDto(), "Toasted bread with garlic and herbs.", new[] { "Bread", "Garlic" }),
                    MenuItemDto.Create("Bruschetta", PriceInfo.CreateByGross(4.20m, RegularyVat, Currency).ToDto(), "Toasted bread with tomatoes and basil.", new[] { "Bread", "Tomato", "Basil" }),
                    MenuItemDto.Create("Mozzarella Sticks", PriceInfo.CreateByGross(4.80m, RegularyVat, Currency).ToDto(), "Fried mozzarella sticks with marinara sauce.", new[] { "Mozzarella", "Breadcrumbs" }),
                    MenuItemDto.Create("Stuffed Mushrooms", PriceInfo.CreateByGross(5.50m, RegularyVat, Currency).ToDto(), "Mushrooms stuffed with cheese and herbs.", new[] { "Mushrooms", "Cheese", "Herbs" }),
                    MenuItemDto.Create("Antipasto Plate", PriceInfo.CreateByGross(6.80m, RegularyVat, Currency).ToDto(), "Assorted meats, cheeses, and olives.", new[] { "Ham", "Salami", "Cheese", "Olives" })
                }
            ),
            MenuSectionDto.Create(
                "Salads",
                new List<MenuItemDto>()
                {
                    MenuItemDto.Create("Caesar Salad", PriceInfo.CreateByGross(6.00m, RegularyVat, Currency).ToDto(), "Classic Caesar with croutons and Parmesan.", new[] { "Lettuce", "Croutons", "Parmesan" }),
                    MenuItemDto.Create("Greek Salad", PriceInfo.CreateByGross(6.50m, RegularyVat, Currency).ToDto(), "Salad with tomatoes, cucumbers, olives, and feta.", new[] { "Tomato", "Cucumber", "Olives", "Feta" }),
                    MenuItemDto.Create("Caprese Salad", PriceInfo.CreateByGross(5.80m, RegularyVat, Currency).ToDto(), "Tomatoes, mozzarella, and basil drizzled with olive oil.", new[] { "Tomato", "Mozzarella", "Basil" }),
                    MenuItemDto.Create("Mixed Salad", PriceInfo.CreateByGross(5.00m, RegularyVat, Currency).ToDto(), "A mix of fresh seasonal vegetables.", new[] { "Lettuce", "Tomato", "Cucumber", "Carrot" }),
                    MenuItemDto.Create("Arugula and Parmesan Salad", PriceInfo.CreateByGross(6.20m, RegularyVat, Currency).ToDto(), "Arugula topped with shaved Parmesan.", new[] { "Arugula", "Parmesan" })
                }
            ),
            MenuSectionDto.Create(
                "Drinks",
                new List<MenuItemDto>()
                {
                    MenuItemDto.Create("Water", PriceInfo.CreateByGross(2.00m, RegularyVat, Currency).ToDto(), "500ml bottled water.", new[] { "Water" }),
                    MenuItemDto.Create("Coca Cola", PriceInfo.CreateByGross(2.50m, RegularyVat, Currency).ToDto(), "500ml Coca Cola.", new[] { "Soda" }),
                    MenuItemDto.Create("Lemonade", PriceInfo.CreateByGross(2.50m, RegularyVat, Currency).ToDto(), "Refreshing lemonade.", new[] { "Lemon", "Sugar", "Water" }),
                    MenuItemDto.Create("Red Wine", PriceInfo.CreateByGross(4.50m, RegularyVat, Currency).ToDto(), "Glass of house red wine.", new[] { "Wine" }),
                    MenuItemDto.Create("Beer", PriceInfo.CreateByGross(3.50m, RegularyVat, Currency).ToDto(), "500ml of local beer.", new[] { "Beer" })
                }
            ),
            MenuSectionDto.Create(
                "Desserts",
                new List<MenuItemDto>()
                {
                    MenuItemDto.Create("Tiramisu", PriceInfo.CreateByGross(4.00m, RegularyVat, Currency).ToDto(), "Classic Italian coffee-flavored dessert.", new[] { "Mascarpone", "Coffee", "Cocoa" }),
                    MenuItemDto.Create("Panna Cotta", PriceInfo.CreateByGross(3.80m, RegularyVat, Currency).ToDto(), "Italian cream dessert with berries.", new[] { "Cream", "Berries" }),
                    MenuItemDto.Create("Gelato", PriceInfo.CreateByGross(3.00m, RegularyVat, Currency).ToDto(), "Italian ice cream, assorted flavors.", new[] { "Ice Cream" }),
                    MenuItemDto.Create("Cannoli", PriceInfo.CreateByGross(4.50m, RegularyVat, Currency).ToDto(), "Pastry shells filled with sweet ricotta.", new[] { "Ricotta", "Pastry" }),
                    MenuItemDto.Create("Chocolate Mousse", PriceInfo.CreateByGross(3.80m, RegularyVat, Currency).ToDto(), "Rich chocolate mousse.", new[] { "Chocolate", "Cream" })
                }
            ),
            MenuSectionDto.Create(
                "Calzones",
                new List<MenuItemDto>()
                {
                    MenuItemDto.Create("Calzone Classico", PriceInfo.CreateByGross(8.50m, RegularyVat, Currency).ToDto(), "Classic calzone with ham and cheese.", new[] { "Ham", "Cheese", "Tomato" }),
                    MenuItemDto.Create("Vegetable Calzone", PriceInfo.CreateByGross(8.20m, RegularyVat, Currency).ToDto(), "Calzone stuffed with vegetables and cheese.", new[] { "Tomato", "Cheese", "Vegetables" }),
                    MenuItemDto.Create("Calzone Pepperoni", PriceInfo.CreateByGross(9.00m, RegularyVat, Currency).ToDto(), "Calzone filled with pepperoni and cheese.", new[] { "Pepperoni", "Cheese", "Tomato" }),
                    MenuItemDto.Create("Calzone Spinach & Ricotta", PriceInfo.CreateByGross(8.70m, RegularyVat, Currency).ToDto(), "Calzone with spinach and ricotta.", new[] { "Spinach", "Ricotta", "Cheese" }),
                    MenuItemDto.Create("Meat Lover's Calzone", PriceInfo.CreateByGross(9.50m, RegularyVat, Currency).ToDto(), "Loaded with assorted meats.", new[] { "Ham", "Pepperoni", "Sausage", "Cheese" })
                }
            )
        }
    );
}
