using POS.Shared.Domain.Generic.Mapper;

namespace POS.Domains.Customer.Domain.Menus.Dtos.Mapper;

public static class MenuItemEntityMapper
{
    public static IReadOnlyList<MenuItem> ToDomain(this IEnumerable<MenuItemEntity> items)
    => items.Select(ToDomain).ToArray();

    public static MenuItem ToDomain(this MenuItemEntity item)
    => new MenuItem(item);

    public static IReadOnlyList<MenuItemEntity> ToEntity(this IEnumerable<MenuItem> entities)
    => entities.Select(ToEntity).ToArray();

    public static MenuItemEntity ToEntity(this MenuItem entity)
    => new MenuItemEntity(
        entity.Name,
        entity.Price.ToDto(),
        entity.Description,
        entity.Incredients
    );
}
