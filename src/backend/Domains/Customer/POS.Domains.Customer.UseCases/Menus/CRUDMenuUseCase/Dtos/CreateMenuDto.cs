namespace POS.Domains.Customer.UseCases.Menus.CRUDMenuUseCase.Dtos;

/// <summary>
/// Dto for creating a new Menu
/// </summary>
/// <param name="Id">ID of the Menu to create.</param>
/// <param name="Currency">The currency of the menu.</param>
/// <param name="Sections">Sections of the new Menu.</param>
public record CreateMenuDto
(
    Guid Id,
    string Currency,
    IReadOnlyList<MenuSectionDto> Sections
);
