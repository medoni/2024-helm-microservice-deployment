using POS.Domains.Customer.Domain.Menus.States;
using POS.Persistence.PostgreSql.Data.Customer;

namespace POS.Persistence.PostgreSql.Mapper.Customer;
internal static class MenuMapper
{
    public static MenuEntity ToEntity(this MenuState state)
    {
        return new MenuEntity(
            state.MenuId,
            state.CreatedAt,
            state.LastChangedAt,
            state.IsActive ? true : null,
            state.IsActive ? state.ActivatedAt : null,
            state.Sections.ToEntity(state.MenuId)
        );
    }

    public static MenuState ToState(this MenuEntity entity)
    {
        return new MenuState(
            entity.Id,
            entity.CreatedAt,
            entity.LastChangedAt,
            entity.Sections.ToSection(),
            entity.IsActive ?? false,
            entity.ActivatedAt
        );
    }
}
