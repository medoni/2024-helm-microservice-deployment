using Microsoft.Extensions.Logging;
using POS.Domains.Customer.Domain.Orders;
using POS.Shared.Persistence.Repositories;
using POS.Shared.Persistence.Repositories.Decorators;

namespace POS.Domains.Customer.Persistence.Orders.Decorators;
/// <summary>
/// Responsible for logging a calls to <see cref="IOrderRepository" />
/// </summary>
public class LoggingOrderRepositoryDecorator : LoggingGenericRepositoryDecorator<Order>, IOrderRepository
{
    /// <summary>
    /// Creates a new <see cref="LoggingOrderRepositoryDecorator"/>.
    /// </summary>
    public LoggingOrderRepositoryDecorator(IGenericRepository<Order> next, ILogger<IGenericRepository<Order>> logger) : base(next, logger)
    {
    }
}
