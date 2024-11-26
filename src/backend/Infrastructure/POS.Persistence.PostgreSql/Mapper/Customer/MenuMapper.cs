using POS.Domains.Customer.Domain.Menus;
using POS.Persistence.PostgreSql.Data.Customer;

namespace POS.Persistence.PostgreSql.Mapper.Customer;
internal static class MenuMapper
{
    public static MenuState ToState(this MenuEntity entity)
    {
        return new MenuState(
            entity.Id,
            entity.CreatedAt,
            entity.LastChangedAt,
            entity.Currency,
            entity.Sections.ToSection(),
            entity.IsActive ?? false,
            entity.ActivatedAt
        );
    }
}
