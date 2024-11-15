namespace POS.Domains.Customer.UseCases.CRUDMenuUseCase.Dtos;

/// <summary>
/// Dto for a Menu section
/// </summary>
/// <param name="Id">Id of the section.</param>
/// <param name="Name">Name of the section.</param>
/// <param name="Items">Items of the section.</param>
public record MenuSectionDto(
    Guid Id,
    string Name,
    IReadOnlyList<MenuItemDto> Items
)
{
    /// <summary>
    /// Creates a new menu section.
    /// </summary>
    public static MenuSectionDto Create(
        string name,
        IReadOnlyList<MenuItemDto> items
    )
    => new MenuSectionDto(Guid.NewGuid(), name, items);
}
