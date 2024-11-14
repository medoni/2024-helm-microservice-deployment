using POS.Domains.Customer.Domain.Menus;
using POS.Domains.Customer.UseCases.CRUDMenuUseCase.Dtos;

namespace POS.Domains.Customer.UseCases.CRUDMenuUseCase.Mapper;
internal static class MenuSectionDtoMapper
{
    public static IReadOnlyList<MenuSection> ToEntities(this IEnumerable<MenuSectionDto> dtos)
    => dtos.Select(ToEntity).ToArray();

    public static MenuSection ToEntity(this MenuSectionDto dto)
    => new MenuSection(
        dto.Id,
        dto.Name,
        dto.Items.ToEntities()
    );

    public static IReadOnlyList<MenuSectionDto> ToDto(this IEnumerable<MenuSection> dtos)
    => dtos.Select(ToDto).ToArray();

    public static MenuSectionDto ToDto(this MenuSection menuSection)
    => new MenuSectionDto(
        menuSection.Id,
        menuSection.Name,
        menuSection.Items.ToDto()
    );
}
