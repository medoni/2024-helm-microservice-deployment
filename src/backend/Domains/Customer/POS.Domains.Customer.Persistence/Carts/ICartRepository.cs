using POS.Domains.Customer.Domain.Carts;
using POS.Shared.Persistence.Repositories;

namespace POS.Domains.Customer.Persistence.Carts;

/// <summary>
/// Definition of an repository operating on carts.
/// </summary>
public interface ICartRepository : IGenericRepository<Cart>
{
}
