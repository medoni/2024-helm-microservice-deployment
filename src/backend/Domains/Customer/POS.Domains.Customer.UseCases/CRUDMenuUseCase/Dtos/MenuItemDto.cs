using POS.Shared.Domain.Generic.Dtos;

namespace POS.Domains.Customer.UseCases.CRUDMenuUseCase.Dtos;

/// <summary>
/// Dto for a single menu item
/// </summary>
/// <param name="Id">Id of the item.</param>
/// <param name="Name">Name of the item.</param>
/// <param name="Price">Unit price of the item.</param>
/// <param name="Description">Description of the item.</param>
/// <param name="Ingredients">Ingredients of the item.</param>
public record MenuItemDto(
    Guid Id,
    string Name,
    MoneyDto Price,
    string Description,
    IReadOnlyList<string> Ingredients
)
{
    /// <summary>
    /// Creates a new menu item.
    /// </summary>
    public static MenuItemDto Create(
        string name,
        MoneyDto price,
        string description,
        IReadOnlyList<string> ingredients
    )
    => new MenuItemDto(Guid.NewGuid(),
        name,
        price,
        description,
        ingredients
    );
}
