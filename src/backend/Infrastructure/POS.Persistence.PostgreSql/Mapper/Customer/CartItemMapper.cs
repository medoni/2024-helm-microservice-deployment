using POS.Domains.Customer.Abstractions.Carts;
using POS.Persistence.PostgreSql.Data.Customer;
using POS.Shared.Domain.Generic.Mapper;

namespace POS.Persistence.PostgreSql.Mapper.Customer;
internal static class CartItemMapper
{
    public static CartItemEntity ToEntity(this CartItem item, CartEntity cart)
    {
        return new CartItemEntity(
            item.Id,
            cart.Id,
            item.MenuItemId,
            item.CreatedAt,
            item.LastChangedAt,
            item.Name,
            item.Description,
            item.UnitPrice.ToDto(),
            item.Quantity
        )
        {
            Cart = cart
        };
    }

    public static List<CartItemEntity> ToEntity(this IEnumerable<CartItem> items, CartEntity cart)
    => items
        .Select(x => x.ToEntity(cart))
        .ToList();

    public static CartItem ToItem(this CartItemEntity entity)
    {
        return new CartItem(
            entity.Id,
            entity.CreatedAt,
            entity.LastChangedAt,
            entity.MenuItemId,
            entity.Name,
            entity.Description,
            entity.UnitPrice.ToDomain(),
            entity.Quantity
        );
    }

    public static IList<CartItem> ToItems(this IEnumerable<CartItemEntity> entities)
    => entities
        .Select(x => x.ToItem())
        .ToList();
}
