using Microsoft.Extensions.Logging;
using POS.Domains.Customer.Domain.Carts;
using POS.Shared.Persistence.Repositories;
using POS.Shared.Persistence.Repositories.Decorators;

namespace POS.Domains.Customer.Persistence.Carts.Decorators;

/// <summary>
/// Responsible for logging a calls to <see cref="ICartRepository" />
/// </summary>
public class LoggingCartRepositoryDecorator : LoggingGenericRepositoryDecorator<Cart>, ICartRepository
{
    /// <summary>
    /// Creates a new <see cref="LoggingCartRepositoryDecorator"/>.
    /// </summary>
    public LoggingCartRepositoryDecorator(IGenericRepository<Cart> next, ILogger<IGenericRepository<Cart>> logger) : base(next, logger)
    {
    }
}
