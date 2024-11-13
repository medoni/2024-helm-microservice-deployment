using POS.Domains.Customer.Domain.Menus.Entities;
using POS.Shared.Domain.Generic;
using POS.Shared.Domain.Generic.Mapper;

namespace POS.Domains.Customer.Domain.Menus;

/// <summary>
/// Menu item.
/// </summary>
/// <param name="Name">Name of the item.</param>
/// <param name="Price">Unit price.</param>
/// <param name="Description">Description of the item.</param>
/// <param name="Ingredients">List of ingredients.</param>
public record MenuItem
(
    string Name,
    Money Price,
    string Description,
    IEnumerable<string> Ingredients
)
{
    /// <summary>
    /// Creates a <see cref="MenuItem" />
    /// </summary>
    public MenuItem(MenuItemEntity entity)
    : this(
        entity.Name,
        entity.Price.ToEntity(),
        entity.Description,
        entity.Ingredients
    )
    {
    }
}
