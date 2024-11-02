namespace POS.Domains.Customer.UseCases.CRUDMenuUseCase.Dtos;

public record MenuSectionDto(
    string Name,
    IReadOnlyList<MenuItemDto> Items
);