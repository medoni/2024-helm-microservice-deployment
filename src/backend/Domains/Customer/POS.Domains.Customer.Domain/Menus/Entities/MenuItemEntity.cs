using POS.Shared.Domain.Generic.Dtos;

namespace POS.Domains.Customer.Domain.Menus.Entities;

/// <summary>
/// Entity to represent a Menu item
/// </summary>
/// <param name="Name">Name of the item.</param>
/// <param name="Price">Unit price of the item.</param>
/// <param name="Description">Description of the item.</param>
/// <param name="Ingredients">List of ingredients.</param>
public record MenuItemEntity
(
    string Name,
    MoneyDto Price,
    string Description,
    IEnumerable<string> Ingredients
);
