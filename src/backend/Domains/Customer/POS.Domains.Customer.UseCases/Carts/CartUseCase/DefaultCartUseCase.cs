using POS.Domains.Customer.Domain.Carts;
using POS.Domains.Customer.Domain.Menus;
using POS.Domains.Customer.Domain.Orders;
using POS.Domains.Customer.Persistence.Menus;
using POS.Domains.Customer.UseCases.Carts.CartUseCase.Dtos;
using POS.Domains.Customer.UseCases.Carts.CartUseCase.Mapper;
using POS.Shared.Infrastructure.Api.Dtos;
using POS.Shared.Infrastructure.Api.Extensions;
using POS.Shared.Persistence.UOW;

namespace POS.Domains.Customer.UseCases.Carts.CartUseCase;
internal class DefaultCartUseCase : ICartUseCase
{
    private readonly UnitOfWorkFactory _uowFactory;
    private readonly IMenuRespository _menuRepository;

    public DefaultCartUseCase(
        UnitOfWorkFactory uowFactory,
        IMenuRespository menuRepository
    )
    {
        _uowFactory = uowFactory ?? throw new ArgumentNullException(nameof(uowFactory));
        _menuRepository = menuRepository ?? throw new ArgumentNullException(nameof(menuRepository));
    }

    public async Task CreateCartAsync(CreateCartDto dto)
    {
        var uow = _uowFactory();
        var menu = await _menuRepository.GetActiveAsync() ?? throw new ArgumentException("No menu is active.");

        var cart = new Cart(
            dto.Id,
            dto.RequestedAt,
            menu
        );
        uow.Add(cart);

        await uow.CommitAsync();
    }

    public async Task<CartDto> GetCartByIdAsync(Guid id)
    {
        var uow = _uowFactory();
        var cart = await uow.GetAsync<Cart>(id);

        return cart.ToDto();
    }

    public async Task<CartItemDto?> AddItemToCartAsync(Guid id, AddItemDto dto)
    {
        var uow = _uowFactory();
        var cart = await uow.GetAsync<Cart>(id);
        var menu = await uow.GetAsync<Menu>(cart.MenuId);

        var cartItem = cart.AddOrUpdateItem(
            dto.RequestedAt,
            menu: menu,
            menuItemId: dto.MenuItemId,
            newQuantity: dto.Quantity
        );

        await uow.CommitAsync();
        return cartItem?.ToDto();
    }

    public async Task<ResultSetDto<CartItemDto>> GetCartItemsAsync(
        Guid id,
        string? token = null
    )
    {
        var uow = _uowFactory();
        var cart = await uow.GetAsync<Cart>(id);

        return cart.Items
            .Select(x => x.ToDto())
            .ToInMemoryPaginatedResultSet(token);
    }

    public async Task<CartItemDto?> UpdateCartItemAsync(Guid id, UpdateItemDto dto)
    {
        var uow = _uowFactory();
        var cart = await uow.GetAsync<Cart>(id);
        var menu = await uow.GetAsync<Menu>(cart.MenuId);

        var cartItem = cart.AddOrUpdateItem(
            dto.RequestedAt,
            menu: menu,
            menuItemId: dto.MenuItemId,
            newQuantity: dto.Quantity
        );

        await uow.CommitAsync();
        return cartItem?.ToDto();
    }

    public async Task<CartCheckedOutDto> CheckoutCartAsync(Guid cartId, CartCheckOutDto dto)
    {
        var uow = _uowFactory();
        var cart = await uow.GetAsync<Cart>(cartId);
        var order = Order.CreateByCartCheckout(
            cart,
            dto.CheckoutAt
        );

        uow.Add(order);
        cart.Checkout(dto.CheckoutAt, order);

        await uow.CommitAsync();

        return new CartCheckedOutDto(
            cart.Id,
            order.Id,
            DateTimeOffset.UtcNow
        );
    }
}
