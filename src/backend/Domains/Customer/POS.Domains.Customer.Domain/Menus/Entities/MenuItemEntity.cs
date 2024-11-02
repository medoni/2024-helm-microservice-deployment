using POS.Shared.Domain.Generic.Dtos;

namespace POS.Domains.Customer.Domain.Menus.Dtos;

public record MenuItemEntity
(
    string Name,
    MoneyDto Price,
    string Description,
    IEnumerable<string> Incredients
);
