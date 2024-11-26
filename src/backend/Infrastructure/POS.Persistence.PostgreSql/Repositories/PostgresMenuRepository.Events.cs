using Microsoft.EntityFrameworkCore;
using POS.Domains.Customer.Domain.Menus.Events;
using POS.Persistence.PostgreSql.Data.Customer;
using POS.Persistence.PostgreSql.Extensions;
using POS.Persistence.PostgreSql.Mapper.Customer;

namespace POS.Persistence.PostgreSql.Repositories;
partial class PostgresMenuRepository
{
    public Task ProcessUncommitedEventAsync(MenuCreatedEvent evt)
    {
        var menu = new MenuEntity(
            evt.MenuId,
            evt.CreatedAt,
            evt.CreatedAt,
            evt.Currency,
            null,
            null,
            evt.Sections.ToEntity(evt.MenuId)
        );

        DbContext.Menus.Add(menu);
        return Task.CompletedTask;
    }

    public async Task ProcessUncommitedEventAsync(MenuSectionsUpdatedEvent evt)
    {
        var menu = (await DbContext.Menus
            .Include(x => x.Sections).ThenInclude(x => x.Items)
            .Where(x => x.Id == evt.MenuId)
            .FirstOrDefaultAsync())
            ?? throw new InvalidOperationException($"Menu with id '{evt.MenuId}' was not found.");

        DbContext.Set<MenuSectionEntity>().RemoveRange(menu.Sections);
        DbContext.Set<MenuItemEntity>().RemoveRange(menu.Sections.SelectMany(x => x.Items));

        menu.Sections.Clear();
        menu.Sections.AddRange(evt.NewSections.ToEntity(menu.Id));

        DbContext.Set<MenuSectionEntity>().AddRange(menu.Sections);
        DbContext.Set<MenuItemEntity>().AddRange(evt.NewSections.SelectMany(x => x.Items.ToEntity(x.Id)));
        menu.LastChangedAt = evt.UpdatedAt;
    }

    public async Task ProcessUncommitedEventAsync(MenuActivatedEvent evt)
    {
        var menu = await DbContext.Menus.FindAsync(evt.MenuId)
            ?? throw new InvalidOperationException($"Menu with id '{evt.MenuId}' was not found.");

        menu.IsActive = true;
        menu.ActivatedAt = evt.ActivatedAt;
        menu.LastChangedAt = evt.ActivatedAt;
    }

    public async Task ProcessUncommitedEventAsync(MenuDeactivatedEvent evt)
    {
        var menu = await DbContext.Menus.FindAsync(evt.MenuId)
            ?? throw new InvalidOperationException($"Menu with id '{evt.MenuId}' was not found.");

        menu.IsActive = null;
        menu.ActivatedAt = null;
        menu.LastChangedAt = evt.DeactivatedAt;
    }
}
