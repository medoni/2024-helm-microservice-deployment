using Microsoft.Extensions.Logging;
using POS.Domains.Customer.Domain.Menus;
using POS.Domains.Customer.Persistence.Carts;
using POS.Shared.Persistence.Repositories;
using POS.Shared.Persistence.Repositories.Decorators;

namespace POS.Domains.Customer.Persistence.Menus.Decorators;

/// <summary>
/// Responsible for logging a calls to <see cref="ICartRepository" />
/// </summary>
public class LoggingMenuRepositoryDecorator : LoggingGenericRepositoryDecorator<Menu>, IMenuRespository
{
    /// <inheritdoc/>
    protected new IMenuRespository Next { get; }

    /// <summary>
    /// Creates a new <see cref="LoggingMenuRepositoryDecorator"/>.
    /// </summary>
    public LoggingMenuRepositoryDecorator(IMenuRespository next, ILogger<IGenericRepository<Menu>> logger) : base(next, logger)
    {
        Next = next;
    }

    /// <inheritdoc/>
    public async Task<Menu?> GetActiveAsync()
    {
        try
        {
            Logger.LogInformation("Getting the active, published menu ...");

            var activeMenu = await Next.GetActiveAsync();

            if (activeMenu != null)
            {
                Logger.LogInformation("Successfully got the active, published menu with id '{menuId}'.", activeMenu.Id);
            }
            else
            {
                Logger.LogInformation("No active, published menu found.");
            }

            return activeMenu;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting the active, published menu.");
            throw;
        }
    }
}
