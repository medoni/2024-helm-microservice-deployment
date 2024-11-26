using POS.Domains.Customer.Domain.Orders;
using POS.Domains.Customer.UseCases.Orders.OrderUseCase.Dtos;

namespace POS.Domains.Customer.UseCases.Orders.OrderUseCase.Mapper;
internal static class OrderMapper
{
    public static OrderDto ToDto(this Order order)
    {
        return new OrderDto(
            order.Id,
            order.CreatedAt,
            order.LastChanegdAt,
            order.State,
            order.OrderItems.ToDto(),
            order.PriceSummary.ToDto()
        );
    }
}
