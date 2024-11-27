using Microsoft.Extensions.Logging;
using POS.Domains.Customer.UseCases.Menus.CRUDMenuUseCase.Dtos;

namespace POS.Domains.Customer.UseCases.Menus.PublishMenuUseCase;
internal class LoggingPublishMenuUseCaseDecorator
(
    IPublishMenuUseCase next,
    ILogger<IPublishMenuUseCase> logger
) : IPublishMenuUseCase
{
    public async Task PublishAsync(Guid id)
    {
        try
        {
            logger.LogInformation("Publishing menu with id '{menuId}'...", id);

            await next.PublishAsync(id);

            logger.LogInformation("Successfully published menu with id '{menuId}'.", id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error publishing menu with id '{menuId}'.", id);
            throw;
        }
    }

    public async Task<MenuDto?> GetActiveAsync()
    {
        try
        {
            logger.LogInformation("Getting the currently active menu...");

            var activeMenu = await next.GetActiveAsync();

            if (activeMenu != null)
            {
                logger.LogInformation("Successfully retrieved the active menu with id '{menuId}'.", activeMenu.Id);
            }
            else
            {
                logger.LogInformation("No active menu found.");
            }

            return activeMenu;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving the currently active menu.");
            throw;
        }
    }
}
