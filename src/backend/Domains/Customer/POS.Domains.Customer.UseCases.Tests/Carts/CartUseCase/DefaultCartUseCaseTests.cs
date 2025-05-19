using NSubstitute;
using NSubstitute.ReceivedExtensions;
using POS.Domains.Customer.Domain.Carts;
using POS.Domains.Customer.Domain.Menus;
using POS.Domains.Customer.Domain.Orders;
using POS.Domains.Customer.Persistence.Menus;
using POS.Domains.Customer.UseCases.Carts.CartUseCase;
using POS.Domains.Customer.UseCases.Carts.CartUseCase.Dtos;
using POS.Domains.Customer.UseCases.Tests.Menus;
using POS.Shared.Persistence.UOW;
using POS.Shared.Testing;

namespace POS.Domains.Customer.UseCases.Tests.Carts.CartUseCase;

[TestFixture]
[Category(TestCategories.Unit)]
public class DefaultCartUseCaseTests
{
    private DefaultCartUseCase Sut { get; set; }
    private UnitOfWorkFactory _uowFactoryMock;
    private IMenuRespository _menuRepositoryMock;
    private IUnitOfWork _uowMock;

    [SetUp]
    public void SetUp()
    {
        _uowMock = Substitute.For<IUnitOfWork>();
        _uowFactoryMock = Substitute.For<UnitOfWorkFactory>();
        _menuRepositoryMock = Substitute.For<IMenuRespository>();

        _uowFactoryMock.Invoke().Returns(_uowMock);

        Sut = new DefaultCartUseCase(
            _uowFactoryMock,
            _menuRepositoryMock
        );
    }

    [Test]
    public async Task CreateCartAsync_WithActiveMenu_ShouldCreateCartAndCommit()
    {
        // arrange
        var dto = new CreateCartDto(
            Guid.NewGuid(),
            DateTimeOffset.UtcNow
        );

        var menu = TestDataGenerator.CreateMenu(active: true);
        _menuRepositoryMock.GetActiveAsync().Returns(menu);

        // act
        await Sut.CreateCartAsync(dto);

        // assert
        _uowMock.Received(1).Add(Arg.Is<Cart>(c =>
            c.Id == dto.Id
        ));
        await _uowMock.Received(1).CommitAsync();
    }

    [Test]
    public void CreateCartAsync_WithNoActiveMenu_ShouldThrowArgumentException()
    {
        // arrange
        var dto = new CreateCartDto(
            Guid.NewGuid(),
            DateTimeOffset.UtcNow
        );

        _menuRepositoryMock.GetActiveAsync().Returns((Menu?)null);

        // act & assert
        Assert.ThrowsAsync<ArgumentException>(() => Sut.CreateCartAsync(dto));
    }

    [Test]
    public async Task GetCartByIdAsync_ShouldReturnCartDto()
    {
        // arrange
        var id = Guid.NewGuid();
        var cart = TestDataGenerator.CreateCart(id);
        _uowMock.GetAsync<Cart>(id).Returns(cart);

        // act
        var result = await Sut.GetCartByIdAsync(id);

        // assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(id));
    }

    [Test]
    public async Task AddItemToCartAsync_ShouldAddItemToCartAndCommit()
    {
        // arrange
        var cartId = Guid.NewGuid();

        var menu = TestDataGenerator.CreateMenu(active: true);
        var cart = TestDataGenerator.CreateCart(cartId, menu: menu);
        _uowMock.GetAsync<Cart>(cartId).Returns(cart);
        _uowMock.GetAsync<Menu>(menu.Id).Returns(menu);

        // act
        var menuItem = menu.Sections.First().Items.First();
        var dto = new AddItemDto(menuItem.Id, DateTimeOffset.UtcNow, 2);
        await Sut.AddItemToCartAsync(cartId, dto);

        // assert
        Assert.That(
            cart.Items.Select(x => new
            {
                x.MenuItemId,
                x.Name,
                x.Description,
                x.UnitPrice,
                x.Quantity
            }),
            Is.EqualTo([new {
                MenuItemId = menuItem.Id,
                Name = menuItem.Name,
                Description = menuItem.Description,
                UnitPrice = menuItem.Price,
                Quantity = 2
            }])
        );
        await _uowMock.Received(1).CommitAsync();
    }

    [Test]
    public async Task GetCartItemsAsync_ShouldReturnCartItems()
    {
        // arrange
        var cartId = Guid.NewGuid();

        var menu = TestDataGenerator.CreateMenu(active: true);
        var cart = TestDataGenerator.CreateCart(cartId, menu: menu);

        var cartItemMenuIds = menu.Sections.SelectMany(x => x.Items).Take(2).Select(x => x.Id);
        foreach (var cartItemMenuId in cartItemMenuIds)
        {
            cart.AddOrUpdateItem(DateTimeOffset.UtcNow, menu, cartItemMenuId, 1);
        }

        _uowMock.GetAsync<Menu>(menu.Id).Returns(menu);
        _uowMock.GetAsync<Cart>(cartId).Returns(cart);

        // act
        var result = await Sut.GetCartItemsAsync(cartId);

        // assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Data.Count, Is.EqualTo(cartItemMenuIds.Count()));
    }

    [Test]
    public async Task UpdateCartItemAsync_ShouldUpdateCartItemAndCommit()
    {
        // arrange
        var cartId = Guid.NewGuid();

        var menu = TestDataGenerator.CreateMenu(active: true);
        var cart = TestDataGenerator.CreateCart(cartId, menu: menu);
        _uowMock.GetAsync<Menu>(menu.Id).Returns(menu);
        _uowMock.GetAsync<Cart>(cartId).Returns(cart);

        var menuItem = menu.Sections.First().Items.First();
        cart.AddOrUpdateItem(DateTimeOffset.UtcNow, menu, menuItem.Id, 1);

        // act
        var dto = new UpdateItemDto(menuItem.Id, DateTimeOffset.UtcNow, 3);
        await Sut.UpdateCartItemAsync(cartId, dto);

        // assert
        Assert.That(
            cart.Items.Select(x => new { x.MenuItemId, x.Quantity }),
            Is.EqualTo([new {
                MenuItemId = menuItem.Id,
                Quantity= 3
            }])
        );
        await _uowMock.Received(1).CommitAsync();
    }

    [Test]
    public async Task CheckoutCartAsync_ShouldCheckoutCartAndReturnResult()
    {
        // arrange
        var cartId = Guid.NewGuid();
        var orderId = Guid.NewGuid();

        var menu = TestDataGenerator.CreateMenu(active: true);
        var cart = TestDataGenerator.CreateCart(cartId, menu: menu);
        _uowMock.GetAsync<Menu>(menu.Id).Returns(menu);
        _uowMock.GetAsync<Cart>(cartId).Returns(cart);

        var menuItem = menu.Sections.First().Items.First();
        cart.AddOrUpdateItem(DateTimeOffset.UtcNow, menu, menuItem.Id, 1);

        // act
        var dto = new CartCheckOutDto(DateTimeOffset.UtcNow);
        var result = await Sut.CheckoutCartAsync(cartId, dto);

        // assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.CartId, Is.EqualTo(cartId));
        Assert.That(result.OrderId, Is.Not.Empty);

        Assert.That(cart.CheckoutInfo, Is.Not.Null);
        _uowMock.Received(1).Add<Order>(Arg.Any<Order>());
        await _uowMock.Received(1).CommitAsync();
    }
}
