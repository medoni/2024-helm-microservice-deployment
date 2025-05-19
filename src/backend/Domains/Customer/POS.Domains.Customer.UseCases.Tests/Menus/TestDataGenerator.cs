using POS.Domains.Customer.Domain.Menus;
using POS.Shared.Domain.Generic;

namespace POS.Domains.Customer.UseCases.Tests.Menus;
internal static class TestDataGenerator
{
    internal static Menu CreateMenu(
        Guid? id = null,
        DateTimeOffset? createdAt = null,
        string currency = "EUR",
        IReadOnlyList<MenuSection>? sections = null
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

        return menu;
    }
}
