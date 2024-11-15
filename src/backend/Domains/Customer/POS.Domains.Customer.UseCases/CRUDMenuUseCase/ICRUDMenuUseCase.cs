using POS.Domains.Customer.UseCases.CRUDMenuUseCase.Dtos;

namespace POS.Domains.Customer.UseCases.CRUDMenuUseCase;

/// <summary>
/// Definition for CRUD operations for a Menus
/// </summary>
public interface ICRUDMenuUseCase
{
    /// <summary>
    /// Creates a new Menu by the given Dto.
    /// </summary>
    Task CreateMenuAsync(CreateMenuDto dto);

    /// <summary>
    /// Updates an existing Menu by the given Dto.
    /// </summary>
    Task UpdateMenuAsync(UpdateMenuDto dto);

    /// <summary>
    /// Returns all stored Menus.
    /// </summary>
    IAsyncEnumerable<MenuDto> GetAllAsync();

    /// <summary>
    /// Returns a Menu it's Id
    /// </summary>
    Task<MenuDto> GetByIdAsync(Guid id);
}
