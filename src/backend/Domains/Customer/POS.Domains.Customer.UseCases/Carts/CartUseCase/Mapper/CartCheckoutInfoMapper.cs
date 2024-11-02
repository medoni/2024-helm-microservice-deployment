using POS.Domains.Customer.Abstractions.Carts;
using POS.Domains.Customer.UseCases.Carts.CartUseCase.Dtos;

namespace POS.Domains.Customer.UseCases.Carts.CartUseCase.Mapper;
internal static class CartCheckoutInfoMapper
{
    public static CartCheckoutInfoDto ToDto(this CartCheckoutInfo info)
    {
        return new CartCheckoutInfoDto(
            info.CheckedOutAt,
            info.OrderId
        );
    }
}
