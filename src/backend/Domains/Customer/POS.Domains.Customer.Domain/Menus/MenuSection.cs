using POS.Domains.Customer.Domain.Menus.Dtos;
using POS.Domains.Customer.Domain.Menus.Dtos.Mapper;

namespace POS.Domains.Customer.Domain.Menus;

public record MenuSection
(
    string Name,
    IEnumerable<MenuItem> Items
)
{
    public MenuSection(MenuSectionEntity entity) :
        this(
            entity.Name,
            entity.Items.ToDomain()
        )
    {

    }
}
