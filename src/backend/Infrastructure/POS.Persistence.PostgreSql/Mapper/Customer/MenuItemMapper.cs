using POS.Domains.Customer.Domain.Menus;
using POS.Persistence.PostgreSql.Data.Customer;
using POS.Shared.Domain.Generic.Mapper;

namespace POS.Persistence.PostgreSql.Mapper.Customer;
internal static class MenuItemMapper
{
    public static MenuItemEntity ToEntity(this MenuItem item, Guid sectionId)
    {
        return new MenuItemEntity(
            item.Id,
            sectionId,
            item.Name,
            item.Description,
            item.Ingredients.ToList(),
            item.Price.ToDto()
        );
    }

    public static ICollection<MenuItemEntity> ToEntity(this IEnumerable<MenuItem> items, Guid sectionId)
    {
        return items
            .Select(x => x.ToEntity(sectionId))
            .ToArray();
    }

    public static MenuItem ToItem(this MenuItemEntity entity)
    {
        return new MenuItem(
            entity.Id,
            entity.Name,
            entity.Price.ToDomain(),
            entity.Description,
            entity.Ingredients
        );
    }

    public static IReadOnlyList<MenuItem> ToItems(this IEnumerable<MenuItemEntity> entities)
    {
        return entities
            .Select(x => x.ToItem())
            .ToArray();
    }
}
