using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using POS.Domains.Customer.Domain.Carts;
using POS.Domains.Customer.Domain.Menus;
using POS.Domains.Customer.Persistence.DynamoDb.Repositories;
using POS.Shared.Domain.Generic;
using POS.Shared.Testing;
using System.Net;

namespace POS.Domains.Customer.Persistence.DynamoDb.Tests.Repositories;
[TestFixture]
[Category(TestCategories.Integration)]
public class CartRepositoryIntegrationTests : BaseDynamoDbRepositoryIntegrationTests
{
    private CartRepository Sut { get; set; }

    [SetUp]
    public void SetUp()
    {
        Sut = new CartRepository(
            DynamoDbContext,
            DynamoDbOperationConfig
        );
    }

    [Test]
    public async Task AddAsync_Should_Store_Item()
    {
        // arrange
        var cart = CreateCart();

        // act
        await Sut.AddAsync(cart);

        // assert
        var storedItem = await DynamoDbClient.GetItemAsync(DynamoDbOperationConfig.OverrideTableName, new() { ["id"] = new AttributeValue(cart.Id.ToString()) });
        Assert.That(storedItem.HttpStatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task UpdateAsync_Should_Store_Item()
    {
        // arrange
        var menu = GetMenu();
        var cart = CreateCart();
        await Sut.AddAsync(cart);
        cart = await Sut.GetByIdAsync(cart.Id);

        cart.AddOrUpdateItem(DateTimeOffset.UtcNow, menu, menu.Sections.First().Items.First().Id, 2);

        // act
        await Sut.UpdateAsync(cart);

        // assert
        var storedCart = await Sut.GetByIdAsync(cart.Id);
        Assert.That(storedCart.Items, Is.Not.Empty);
    }

    [Test]
    public async Task GetByIdAsync_Should_Return_Stored_Menu()
    {
        // arrange
        var cart = CreateCart();
        await Sut.AddAsync(cart);

        // act
        var result = await Sut.GetByIdAsync(cart.Id);

        // assert
        Assert.That(result.GetCurrentState<CartState>(), Is.EqualTo(cart.GetCurrentState<CartState>()).UsingJson());
    }

    [Test]
    public async Task IterateAsync_Should_Return_Stored_Menus()
    {
        // arrange
        var cart1 = CreateCart();
        await Sut.AddAsync(cart1);

        var cart2 = CreateCart();
        await Sut.AddAsync(cart2);

        // act
        var result = await Sut.IterateAsync()
            .Select(x => x.Id)
            .ToListAsync();

        // assert
        Assert.That(result, Is.SupersetOf(new[] { cart1.Id, cart2.Id }));
    }

    #region Helpers

    private static Cart CreateCart(
        Guid? id = null,
        DateTimeOffset? createdAt = null,
        List<(Guid menuItemId, int quantity)>? items = null
    )
    {
        var menu = GetMenu();
        var menuItems = menu.Sections.SelectMany(x => x.Items);
        items ??= new() { (menuItems.First().Id, 2) };

        var cart = new Cart(
            id ?? Guid.NewGuid(),
            createdAt ?? DateTimeOffset.UtcNow,
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
        try
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
        catch (Exception ex)
        {
            var logs = await DynamoDbContainer.GetLogsAsync();

            throw new InvalidOperationException(
                $"""
                DynamoDbClient.GetConnectionString(): '{DynamoDbContainer.GetConnectionString()}'
                -------------------------------------------
                Stdout:
                {logs.Stdout}
                -------------------------------------------
                Stderr:
                {logs.Stderr}
                -------------------------------------------
                """, ex
            );
        }
    }

    #endregion
}
