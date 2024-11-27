using Microsoft.Extensions.Logging;
using POS.Domains.Customer.UseCases.Menus.CRUDMenuUseCase.Dtos;

namespace POS.Domains.Customer.UseCases.Menus.CRUDMenuUseCase;
internal class LoggingCRUDMenuUseCaseDecorator(
    ICRUDMenuUseCase next,
    ILogger<ICRUDMenuUseCase> logger
) : ICRUDMenuUseCase
{
    public async Task CreateMenuAsync(CreateMenuDto dto)
    {
        try
        {
            logger.LogInformation("Creating new menu with id '{menudId}' ...", dto.Id);

            await next.CreateMenuAsync(dto);

            logger.LogInformation("Successfully created new menu with id '{menuId}'.", dto.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating new menu with id '{menuId}'.", dto.Id);
            throw;
        }
    }

    public IAsyncEnumerable<MenuDto> GetAllAsync()
    {
        try
        {
            logger.LogInformation("Getting all menus ienumerable ...");

            var result = next.GetAllAsync();

            logger.LogInformation("Successfully got menus ienumerable.");
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting menus ienumerable.");
            throw;
        }
    }

    public async Task<MenuDto> GetByIdAsync(Guid id)
    {
        try
        {
            logger.LogInformation("Getting menu with id '{menudId}' ...", id);

            var menu = await next.GetByIdAsync(id);

            logger.LogInformation("Successfully got menu with id '{menuId}'.", id);

            return menu;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting menu with id '{menuId}'.", id);
            throw;
        }
    }

    public async Task UpdateMenuAsync(UpdateMenuDto dto)
    {
        try
        {
            logger.LogInformation("Updating menu with id '{menudId}' ...", dto.Id);

            await next.UpdateMenuAsync(dto);

            logger.LogInformation("Successfully updated menu with id '{menuId}'.", dto.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating menu with id '{menuId}'.", dto.Id);
            throw;
        }
    }
}
