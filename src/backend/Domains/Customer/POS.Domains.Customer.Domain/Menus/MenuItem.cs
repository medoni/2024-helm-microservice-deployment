using POS.Domains.Customer.Domain.Menus.Dtos;
using POS.Shared.Domain.Generic;
using POS.Shared.Domain.Generic.Mapper;

namespace POS.Domains.Customer.Domain.Menus;

public record MenuItem
(
    string Name,
    Money Price,
    string Description,
    IEnumerable<string> Incredients
)
{
    public MenuItem(MenuItemEntity entity)
    : this(
        entity.Name,
        entity.Price.ToEntity(),
        entity.Description,
        entity.Incredients
    )
    {
    }
}
