using POS.Domains.Customer.Domain.Carts;
using POS.Domains.Customer.Domain.Menus;
using POS.Shared.Domain.Generic;

namespace POS.Domains.Customer.UseCases.Tests.Menus;
internal static class TestDataGenerator
{
    internal static Menu CreateMenu(
        Guid? id = null,
        DateTimeOffset? createdAt = null,
        string currency = "EUR",
        IReadOnlyList<MenuSection>? sections = null,
        bool active = false
    )
    {
        sections ??= new List<MenuSection>()
        {
            new MenuSection(Guid.NewGuid(), "Example-Section", new List<MenuItem>
            {
                new MenuItem(Guid.NewGuid(), "Example-Item", PriceInfo.CreateByGross(10, 7, currency), "Description of example item", new[] { "Ingredients 1" })
            })
        };

        var menu = new Menu(
            id ?? Guid.NewGuid(),
            createdAt ?? DateTimeOffset.UtcNow,
            currency,
            sections
        );

        if (active)
        {
            menu.Activate(DateTimeOffset.UtcNow);
        }

        return menu;
    }

    internal static Cart CreateCart(
        Guid? id = null,
        DateTimeOffset? createdAt = null,
        Menu? menu = null
    )
    {
        menu ??= CreateMenu(active: true);

        var cart = new Cart(
            id ?? Guid.NewGuid(),
            createdAt ?? DateTimeOffset.UtcNow,
            menu
        );

        return cart;
    }
}
