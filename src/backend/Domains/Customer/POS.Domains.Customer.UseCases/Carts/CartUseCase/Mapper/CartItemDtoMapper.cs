using POS.Domains.Customer.Abstractions.Carts;
using POS.Domains.Customer.UseCases.Carts.CartUseCase.Dtos;
using POS.Shared.Domain.Generic.Mapper;

namespace POS.Domains.Customer.UseCases.Carts.CartUseCase.Mapper;
internal static class CartItemDtoMapper
{
    public static CartItemDto ToDto(this CartItem item)
    => new CartItemDto(
        item.Id,
        item.CreatedAt,
        item.Name,
        item.Description,
        item.UnitPrice.ToDto(),
        item.Quantity
    );
}
