using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using POS.Domains.Customer.Domain.Carts;
using POS.Domains.Customer.Domain.Menus;
using POS.Domains.Customer.Domain.Orders;
using POS.Domains.Customer.Persistence.DynamoDb.Repositories;
using POS.Shared.Domain.Generic;
using POS.Shared.Testing;
using System.Net;

namespace POS.Domains.Customer.Persistence.DynamoDb.Tests.Repositories;
[TestFixture]
[Category(TestCategories.Integration)]
public class OrderRepositoryIntegrationTests : BaseDynamoDbRepositoryFixture
{
    private OrderRepository Sut { get; set; }

    [SetUp]
    public void SetUp()
    {
        Sut = new OrderRepository(
            DynamoDbContext,
            DynamoDbOperationConfig
        );
    }

    [Test]
    public async Task AddAsync_Should_Store_Item()
    {
        // arrange
        var order = CreateOrder();

        // act
        await Sut.AddAsync(order);

        // assert
        var storedItem = await DynamoDbClient.GetItemAsync(DynamoDbOperationConfig.OverrideTableName, new() { ["id"] = new AttributeValue(order.Id.ToString()) });
        Assert.That(storedItem.HttpStatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task GetByIdAsync_Should_Return_Stored_Menu()
    {
        // arrange
        var order = CreateOrder();
        await Sut.AddAsync(order);

        // act
        var result = await Sut.GetByIdAsync(order.Id);

        // assert
        Assert.That(result.GetCurrentState<OrderState>(), Is.EqualTo(order.GetCurrentState<OrderState>()).UsingJson());
    }

    [Test]
    public async Task IterateAsync_Should_Return_Stored_Menus()
    {
        // arrange
        var order1 = CreateOrder();
        await Sut.AddAsync(order1);

        var order2 = CreateOrder();
        await Sut.AddAsync(order2);

        // act
        var result = await Sut.IterateAsync()
            .Select(x => x.Id)
            .ToListAsync();

        // assert
        Assert.That(result, Is.SupersetOf(new[] { order1.Id, order2.Id }));
    }

    #region Helpers

    private static Order CreateOrder(
        Guid? id = null,
        DateTimeOffset? createdAt = null
    )
    {
        var cart = CreateCart();

        var order = Order.CreateByCartCheckout(
            cart,
            DateTimeOffset.UtcNow
        );

        return order;
    }

    private static Cart CreateCart()
    {
        var menu = GetMenu();
        var menuItems = menu.Sections.SelectMany(x => x.Items);
        var items = new List<(Guid menuItemId, int quantity)> { (menuItems.First().Id, 2) };

        var cart = new Cart(
            Guid.NewGuid(),
            DateTimeOffset.UtcNow,
            menu
        );

        foreach (var item in items)
        {
            cart.AddOrUpdateItem(cart.CreatedAt, menu, item.menuItemId, item.quantity);
        }

        return cart;
    }

    private static Menu GetMenu()
    {
        var currency = "EUR";
        var sections = new List<MenuSection>()
        {
            new MenuSection(Guid.NewGuid(), "Example-Section", new List<MenuItem>
            {
                new MenuItem(Guid.NewGuid(), "Example-Item", PriceInfo.CreateByGross(10, 7, currency), "Description of example item", new[] { "Ingredients 1" })
            })
        };

        var menu = new Menu(
            Guid.Parse("1b02c69d-6c7d-4eec-b569-a16c85b6adea"),
            new DateTimeOffset(2024, 12, 09, 14, 48, 33, TimeSpan.FromHours(1)),
            currency,
            sections
        );

        menu.Activate(menu.CreatedAt);

        return menu;
    }

    protected override async Task CreateTableAsync(DynamoDBOperationConfig operationConfig)
    {
        var createTableRequest = new CreateTableRequest
        {
            TableName = operationConfig.OverrideTableName,
            BillingMode = BillingMode.PAY_PER_REQUEST,
            KeySchema = new()
            {
                new KeySchemaElement("id", KeyType.HASH)
            },
            AttributeDefinitions = new()
            {
                new AttributeDefinition() { AttributeName = "id", AttributeType = "S" }
            }
        };
        await DynamoDbClient.CreateTableAsync(createTableRequest);
    }

    #endregion
}
