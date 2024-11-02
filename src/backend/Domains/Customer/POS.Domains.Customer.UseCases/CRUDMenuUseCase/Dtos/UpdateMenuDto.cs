namespace POS.Domains.Customer.UseCases.CRUDMenuUseCase.Dtos;

public record UpdateMenuDto
(
    Guid Id,
    IReadOnlyList<MenuSectionDto> Sections
);
