﻿using POS.Domains.Customer.Domain.Menus;
using POS.Domains.Customer.UseCases.Menus.CRUDMenuUseCase.Dtos;
using POS.Domains.Customer.UseCases.Menus.CRUDMenuUseCase.Mapper;
using POS.Shared.Domain.Generic.Mapper;

namespace POS.Domains.Customer.UseCases.Menus.CRUDMenuUseCase.Mapper;
internal static class MenuItemDtoMapper
{
    public static IReadOnlyList<MenuItem> ToEntities(this IEnumerable<MenuItemDto> dtos)
    => dtos.Select(ToEntity).ToArray();

    public static MenuItem ToEntity(this MenuItemDto dto)
    => new MenuItem(
        dto.Id,
        dto.Name,
        dto.Price.ToDomain(),
        dto.Description,
        dto.Ingredients
    );

    public static IReadOnlyList<MenuItemDto> ToDto(this IEnumerable<MenuItem> dtos)
    => dtos.Select(ToDto).ToArray();

    public static MenuItemDto ToDto(this MenuItem menuItem)
    => new MenuItemDto(
        menuItem.Id,
        menuItem.Name,
        menuItem.Price.ToDto(),
        menuItem.Description,
        menuItem.Ingredients
    );
}
