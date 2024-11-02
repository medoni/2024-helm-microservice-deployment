using POS.Domains.Customer.Domain.Carts;
using POS.Domains.Customer.UseCases.Carts.CartUseCase.Dtos;
using POS.Shared.Domain.Generic.Mapper;

namespace POS.Domains.Customer.UseCases.Carts.CartUseCase.Mapper;
internal static class CartDtoMapper
{
    public static CartDto ToDto(this Cart cart)
    => new CartDto(
        cart.Id,
        cart.CreatedAt,
        cart.LastChangedAt,
        cart.State,
        cart.MenuId,
        cart.Items.Count,
        cart.CalculateTotalPrice().ToDto(),
        cart.CheckoutInfo?.ToDto()
    );
}
