using POS.Domains.Customer.Abstractions.Orders;
using POS.Persistence.PostgreSql.Data.Customer;

namespace POS.Persistence.PostgreSql.Mapper.Customer;
internal static class OrderItemMapper
{
    public static OrderItem ToDomain(this OrderItemEntity entity)
    {
        return new OrderItem(
            entity.Id,
            entity.CartItemId,
            entity.Name,
            entity.Description,
            entity.UnitPrice,
            entity.Quantity,
            entity.TotalPrice
        );
    }

    public static IReadOnlyList<OrderItem> ToDomain(this IEnumerable<OrderItemEntity> items)
    => items
        .Select(x => x.ToDomain())
        .ToList();

    public static OrderItemEntity ToEntity(
        this OrderItem domain,
        Guid orderId
    )
    {
        return new OrderItemEntity(
            domain.ItemId,
            orderId,
            domain.CartItemId,
            domain.Name,
            domain.Description,
            domain.UnitPrice,
            domain.Quantity,
            domain.TotalPrice
        );
    }

    public static ICollection<OrderItemEntity> ToEntity(
        this IEnumerable<OrderItem> items,
        Guid orderId
    )
    => items
        .Select(x => x.ToEntity(orderId))
        .ToList();
}
