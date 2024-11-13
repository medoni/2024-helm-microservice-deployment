using POS.Shared.Domain.Generic.Dtos;

namespace POS.Domains.Customer.UseCases.CRUDMenuUseCase.Dtos;

/// <summary>
/// Dto for a single menu item
/// </summary>
/// <param name="Name">Name of the item.</param>
/// <param name="Price">Unit price of the item.</param>
/// <param name="Description">Description of the item.</param>
/// <param name="Ingredients">Ingredients of the item.</param>
public record MenuItemDto(
    string Name,
    MoneyDto Price,
    string Description,
    IEnumerable<string> Ingredients
);
