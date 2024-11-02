using POS.Domains.Customer.Domain.Menus;
using POS.Shared.Persistence.Repositories;

namespace POS.Domains.Customer.Persistence.Menus;

public interface IMenuRespository : IGenericRepository<Menu, Guid>
{
    Task<Menu?> GetActiveAsync();
}
