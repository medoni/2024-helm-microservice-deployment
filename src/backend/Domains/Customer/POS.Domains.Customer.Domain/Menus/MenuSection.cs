using POS.Domains.Customer.Domain.Menus.Entities;
using POS.Domains.Customer.Domain.Menus.Entities.Mapper;

namespace POS.Domains.Customer.Domain.Menus;

/// <summary>
/// Menu Section.
/// </summary>
/// <param name="Name">Name of the section.</param>
/// <param name="Items">List of Menu items.</param>
public record MenuSection
(
    string Name,
    IEnumerable<MenuItem> Items
)
{
    /// <summary>
    /// Creates a new <see cref="MenuSection"/>.
    /// </summary>
    public MenuSection(MenuSectionEntity entity) :
        this(
            entity.Name,
            entity.Items.ToDomain()
        )
    {

    }
}
