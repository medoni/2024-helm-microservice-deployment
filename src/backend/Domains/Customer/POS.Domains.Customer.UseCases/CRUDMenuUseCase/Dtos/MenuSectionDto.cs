namespace POS.Domains.Customer.UseCases.CRUDMenuUseCase.Dtos;

/// <summary>
/// Dto for a Menu section
/// </summary>
/// <param name="Name">Name of the section.</param>
/// <param name="Items">Items of the section.</param>
public record MenuSectionDto(
    string Name,
    IReadOnlyList<MenuItemDto> Items
);
