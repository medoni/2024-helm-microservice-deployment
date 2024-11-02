namespace POS.Domains.Customer.Domain.Menus.Dtos;

public record MenuSectionEntity
(
    string Name,
    IEnumerable<MenuItemEntity> Items
);
