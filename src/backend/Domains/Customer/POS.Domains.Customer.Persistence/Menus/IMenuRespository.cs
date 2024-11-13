using POS.Domains.Customer.Domain.Menus;
using POS.Shared.Persistence.Repositories;

namespace POS.Domains.Customer.Persistence.Menus;

/// <summary>
/// Definition of an Repository to operate with Menus
/// </summary>
public interface IMenuRespository : IGenericRepository<Menu, Guid>
{
    /// <summary>
    /// Returns the active, published, Menu
    /// </summary>
    Task<Menu?> GetActiveAsync();
}
