namespace POS.Domains.Customer.UseCases.CRUDMenuUseCase.Dtos;

public record CreateMenuDto
(
    Guid Id,
    IReadOnlyList<MenuSectionDto> Sections
);
