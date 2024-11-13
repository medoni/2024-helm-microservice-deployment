using POS.Domains.Customer.Domain.Menus;
using POS.Domains.Customer.UseCases.CRUDMenuUseCase.Dtos;

namespace POS.Domains.Customer.UseCases.CRUDMenuUseCase.Mapper;
internal static class MenuDtoMapper
{
    public static MenuDto ToDto(this Menu menu)
    => new MenuDto(
        menu.Id,
        menu.CreatedAt,
        menu.LastChangedAt,
        menu.Sections.ToDto()
    )
    {
        IsActive = menu.IsActive,
        ActivatedAt = menu.ActivatedAt
    };
}
