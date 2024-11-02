namespace POS.Domains.Customer.Domain.Menus.Dtos.Mapper;

public static class MenuSectionEntityMapper
{
    public static IReadOnlyList<MenuSection> ToDomain(this IEnumerable<MenuSectionEntity> items)
    => items.Select(ToDomain).ToArray();

    public static MenuSection ToDomain(this MenuSectionEntity item)
    => new MenuSection(item);

    public static List<MenuSectionEntity> ToEntity(this IEnumerable<MenuSection> entities)
    => entities.Select(ToEntity).ToList();

    public static MenuSectionEntity ToEntity(this MenuSection entity)
    => new MenuSectionEntity(
        entity.Name,
        entity.Items.ToEntity()
    );
}
