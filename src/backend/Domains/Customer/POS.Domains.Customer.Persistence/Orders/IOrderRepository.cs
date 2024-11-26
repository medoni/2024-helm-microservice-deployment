using POS.Domains.Customer.Domain.Orders;
using POS.Shared.Persistence.Repositories;

namespace POS.Domains.Customer.Persistence.Orders;

/// <summary>
/// Definition of a repository to operate on <see cref="Order"/>s
/// </summary>
public interface IOrderRepository : IGenericRepository<Order>
{
}
