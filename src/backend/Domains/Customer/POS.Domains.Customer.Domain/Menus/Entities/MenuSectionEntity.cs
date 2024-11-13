namespace POS.Domains.Customer.Domain.Menus.Entities;

/// <summary>
/// Entity to represent a Menu section.
/// </summary>
/// <param name="Name">Name of the section.</param>
/// <param name="Items">List of Menu items.</param>
public record MenuSectionEntity
(
    string Name,
    IEnumerable<MenuItemEntity> Items
);
