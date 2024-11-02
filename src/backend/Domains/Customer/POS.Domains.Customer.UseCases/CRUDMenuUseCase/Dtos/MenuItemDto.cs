using POS.Shared.Domain.Generic.Dtos;

namespace POS.Domains.Customer.UseCases.CRUDMenuUseCase.Dtos;

public record MenuItemDto(
    string Name,
    MoneyDto Price,
    string Description,
    IEnumerable<string> Incredients
);
