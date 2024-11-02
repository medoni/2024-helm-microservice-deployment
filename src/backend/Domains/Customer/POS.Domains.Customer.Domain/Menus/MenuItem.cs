using POS.Shared.Domain.Generic;

namespace POS.Domains.Customer.Domain.Menus;

/// <summary>
/// Menu item.
/// </summary>
/// <param name="Id">Id of the item.</param>
/// <param name="Name">Name of the item.</param>
/// <param name="Price">Unit price.</param>
/// <param name="Description">Description of the item.</param>
/// <param name="Ingredients">List of ingredients.</param>
public record MenuItem
(
    Guid Id,
    string Name,
    PriceInfo Price,
    string Description,
    IReadOnlyList<string> Ingredients
)
{
}
