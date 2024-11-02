using POS.Domains.Customer.UseCases.Carts.CartUseCase;
using POS.Domains.Customer.UseCases.Carts.CartUseCase.Dtos;
using POS.Domains.Customer.UseCases.Menus.CRUDMenuUseCase.Dtos;
using POS.Domains.Customer.UseCases.Menus.PublishMenuUseCase;
using POS.Shared.Persistence.PostgreSql.DbSeeds;

namespace POS.Persistence.DbSeed.Seeds;
internal class _20241116_AddCarts_Seed : ISeeder
{
    private readonly ICartUseCase _cartsService;
    private readonly IPublishMenuUseCase _menuPublishService;

    private static Random Random = Random.Shared;

    public _20241116_AddCarts_Seed(
        ICartUseCase cartsService,
        IPublishMenuUseCase menuPublishService
    )
    {
        _cartsService = cartsService ?? throw new ArgumentNullException(nameof(cartsService));
        _menuPublishService = menuPublishService ?? throw new ArgumentNullException(nameof(menuPublishService));
    }

    public string Name => "Add carts seed";

    public DateTimeOffset AddedAt => new DateTime(2011, 11, 16, 15, 05, 00, DateTimeKind.Utc);

    public async Task SeedAsync()
    {
        var activeMenu = await _menuPublishService.GetActiveAsync() ?? throw new InvalidOperationException("No active menu found.");
        var menuItems = activeMenu.Sections
            .SelectMany(x => x.Items)
            .ToArray();

        var count = Random.Next(30, 200);
        foreach (var idx in Enumerable.Range(1, count))
        {
            await AddCartAsync(menuItems);
        }
    }

    private async Task AddCartAsync(
        IReadOnlyList<MenuItemDto> menuItems
    )
    {
        var createCartDto = new CreateCartDto(
            Guid.NewGuid(),
            DateTimeOffset.UtcNow
        );
        await _cartsService.CreateCartAsync(createCartDto);

        var itemCount = Random.Next(1, 10);
        var totalQuantity = 0;
        foreach (var item in Enumerable.Range(1, itemCount))
        {
            var menuItem = menuItems[Random.Next(0, menuItems.Count)];
            var quantity = Random.Next(1, 5);
            var addItemDto = new AddItemDto(menuItem.Id, DateTimeOffset.UtcNow, quantity);
            await _cartsService.AddItemToCartAsync(createCartDto.Id, addItemDto);

            totalQuantity += quantity;
        }

        await CheckItemsAsync(createCartDto.Id, totalQuantity);
    }

    private async Task CheckItemsAsync(Guid cartId, int expectedTotalQuantity)
    {
        var cartItemsDto = await _cartsService.GetCartItemsAsync(cartId);
        var totalQuantity = cartItemsDto.Data.Sum(x => x.Quantity);
        if (totalQuantity != expectedTotalQuantity) throw new InvalidOperationException($"Expected quantity: {expectedTotalQuantity}, got: {totalQuantity}");
    }
}
