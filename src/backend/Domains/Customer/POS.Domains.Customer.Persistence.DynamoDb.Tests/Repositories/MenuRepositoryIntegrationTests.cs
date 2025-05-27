using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using POS.Domains.Customer.Api.Dtos.Examples;
using POS.Domains.Customer.Domain.Menus;
using POS.Domains.Customer.Persistence.DynamoDb.Repositories;
using POS.Domains.Customer.UseCases.Menus.CRUDMenuUseCase.Mapper;
using POS.Shared.Domain.Generic;
using POS.Shared.Testing;
using System.Net;

namespace POS.Domains.Customer.Persistence.DynamoDb.Tests.Repositories;

[TestFixture]
[Category(TestCategories.Integration)]
public class MenuRepositoryIntegrationTests : BaseDynamoDbRepositoryFixture
{
    private MenuRepository Sut { get; set; }

    [SetUp]
    public void SetUp()
    {
        Sut = new MenuRepository(
            DynamoDbContext,
            DynamoDbOperationConfig
        );
    }

    [Test]
    public async Task AddAsync_Should_Successful_Store_Example_Menu()
    {
        // arrange
        var menuDto = new CreateMenuDtoExample().GetExamples();
        var menu = new Menu(
            menuDto.Id,
            DateTimeOffset.UtcNow,
            menuDto.Currency,
            menuDto.Sections.ToEntities()
        );

        // act
        await Sut.AddAsync(menu);

        // assert
        var storedItem = await DynamoDbClient.GetItemAsync(DynamoDbOperationConfig.OverrideTableName, new() { ["id"] = new AttributeValue(menuDto.Id.ToString()) });
        Assert.That(storedItem.HttpStatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task GetByIdAsync_Should_Return_Stored_Menu()
    {
        // arrange
        var menu = CreateMenu();
        await Sut.AddAsync(menu);

        // act
        var result = await Sut.GetByIdAsync(menu.Id);

        // assert
        Assert.That(result.GetCurrentState<MenuState>(), Is.EqualTo(menu.GetCurrentState<MenuState>()).UsingJson());
    }

    [Test]
    public async Task GetActiveAsync_Should_Return_Active_Menu()
    {
        // arrange
        var menu = CreateMenu();
        menu.Activate(DateTimeOffset.UtcNow);
        await Sut.AddAsync(menu);

        // act
        var result = await Sut.GetActiveAsync();

        // assert
        Assert.That(result?.IsActive, Is.True);
    }

    [Test]
    public async Task IterateAsync_Should_Return_Stored_Menus()
    {
        // arrange
        var menu1 = CreateMenu();
        await Sut.AddAsync(menu1);

        var menu2 = CreateMenu();
        await Sut.AddAsync(menu2);

        // act
        var result = await Sut.IterateAsync()
            .Select(x => x.Id)
            .ToListAsync();

        // assert
        Assert.That(result, Is.SupersetOf(new[] { menu1.Id, menu2.Id }));
    }

    [Test]
    public async Task Update_Without_Any_Changes_Should_Not_Store_Data()
    {
        // arrange
        var menu = CreateMenu();
        await Sut.AddAsync(menu);

        // act
        menu = await Sut.GetByIdAsync(menu.Id);
        await Sut.UpdateAsync(menu);

        // assert
        Assert.Fail();
    }

    #region Helpers

    private static Menu CreateMenu(
        Guid? id = null,
        DateTimeOffset? createdAt = null,
        string currency = "EUR",
        IReadOnlyList<MenuSection>? sections = null
    )
    {
        sections ??= new List<MenuSection>()
        {
            new MenuSection(Guid.NewGuid(), "Example-Section", new List<MenuItem>
            {
                new MenuItem(Guid.NewGuid(), "Example-Item", PriceInfo.CreateByGross(10, 7, currency), "Description of example item", new[] { "Ingredients 1" })
            })
        };

        var menu = new Menu(
            id ?? Guid.NewGuid(),
            createdAt ?? DateTimeOffset.UtcNow,
            currency,
            sections
        );

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
                new AttributeDefinition() { AttributeName = "id", AttributeType = "S" },
                new AttributeDefinition() { AttributeName = "active", AttributeType = "N" }
            },
            GlobalSecondaryIndexes = new List<GlobalSecondaryIndex>()
            {
                new GlobalSecondaryIndex()
                {
                    IndexName = "active",
                    KeySchema = new List<KeySchemaElement>()
                    {
                        new KeySchemaElement("active", KeyType.HASH)
                    },
                    Projection = new Projection()
                    {
                        ProjectionType = ProjectionType.INCLUDE,
                        NonKeyAttributes = new List<string>()
                        {
                            "active",
                            "payload"
                        }
                    }
                }
            }
        };
        await DynamoDbClient.CreateTableAsync(createTableRequest);
    }

    #endregion
}
