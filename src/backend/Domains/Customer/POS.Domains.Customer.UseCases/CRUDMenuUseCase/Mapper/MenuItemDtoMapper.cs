using POS.Domains.Customer.Domain.Menus;
using POS.Domains.Customer.UseCases.CRUDMenuUseCase.Dtos;
using POS.Shared.Domain.Generic.Mapper;

namespace POS.Domains.Customer.UseCases.CRUDMenuUseCase.Mapper;
public static class MenuItemDtoMapper
{
    public static IReadOnlyList<MenuItem> ToEntities(this IEnumerable<MenuItemDto> dtos)
    => dtos.Select(ToEntity).ToArray();

    public static MenuItem ToEntity(this MenuItemDto dto)
    => new MenuItem(
        dto.Name,
        dto.Price.ToEntity(),
        dto.Description,
        dto.Incredients
    );

    public static IReadOnlyList<MenuItemDto> ToDto(this IEnumerable<MenuItem> dtos)
    => dtos.Select(ToDto).ToArray();

    public static MenuItemDto ToDto(this MenuItem menuItem)
    => new MenuItemDto(
        menuItem.Name,
        menuItem.Price.ToDto(),
        menuItem.Description,
        menuItem.Incredients
    );
}
