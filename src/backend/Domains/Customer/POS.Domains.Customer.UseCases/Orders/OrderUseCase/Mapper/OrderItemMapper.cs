using POS.Domains.Customer.Abstractions.Orders;
using POS.Domains.Customer.UseCases.Orders.OrderUseCase.Dtos;

namespace POS.Domains.Customer.UseCases.Orders.OrderUseCase.Mapper;
internal static class OrderItemMapper
{
    public static OrderItemDto ToDto(this OrderItem item)
    {
        return new OrderItemDto(
            item.ItemId,
            item.CartItemId,
            item.Name,
            item.Description,
            item.UnitPrice,
            item.Quantity,
            item.TotalPrice
        );
    }

    public static List<OrderItemDto> ToDto(this IEnumerable<OrderItem> items)
    => items
        .Select(ToDto)
        .ToList();
}
