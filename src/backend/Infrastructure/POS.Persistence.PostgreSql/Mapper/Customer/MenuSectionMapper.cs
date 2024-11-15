using POS.Domains.Customer.Domain.Menus;
using POS.Persistence.PostgreSql.Data.Customer;

namespace POS.Persistence.PostgreSql.Mapper.Customer;
internal static class MenuSectionMapper
{
    public static MenuSectionEntity ToEntity(this MenuSection section, Guid menuId)
    {
        return new MenuSectionEntity(
            section.Id,
            menuId,
            section.Name,
            section.Items.ToEntity(section.Id)
        );
    }

    public static ICollection<MenuSectionEntity> ToEntity(this IEnumerable<MenuSection> sections, Guid menuId)
    {
        return sections
            .Select(x => x.ToEntity(menuId))
            .ToList();
    }

    public static MenuSection ToSection(this MenuSectionEntity entity)
    {
        return new MenuSection(
            entity.Id,
            entity.Name,
            entity.Items.ToItems()
        );
    }

    public static IReadOnlyList<MenuSection> ToSection(this IEnumerable<MenuSectionEntity> entities)
    {
        return entities
            .Select(x => x.ToSection())
            .ToArray();
    }
}
