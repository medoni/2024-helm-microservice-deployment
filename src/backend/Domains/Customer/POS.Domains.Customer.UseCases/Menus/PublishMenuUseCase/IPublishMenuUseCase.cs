using POS.Domains.Customer.UseCases.Menus.CRUDMenuUseCase.Dtos;

namespace POS.Domains.Customer.UseCases.Menus.PublishMenuUseCase;

/// <summary>
/// Definitions for handling active Menus.
/// </summary>
public interface IPublishMenuUseCase
{
    /// <summary>
    /// Publishes an existing Menu and makes it `active`.
    /// </summary>
    Task PublishAsync(Guid id);

    /// <summary>
    /// Returns the active Menu or null if no Menu is currently active.
    /// </summary>
    Task<MenuDto?> GetActiveAsync();
}
