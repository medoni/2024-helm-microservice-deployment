namespace POS.Domains.Customer.UseCases.CRUDMenuUseCase.Dtos;

/// <summary>
/// Dto for updating a Menu
/// </summary>
/// <param name="Id">Id to update.</param>
/// <param name="Sections">New sections</param>
public record UpdateMenuDto
(
    Guid Id,
    IReadOnlyList<MenuSectionDto> Sections
);
