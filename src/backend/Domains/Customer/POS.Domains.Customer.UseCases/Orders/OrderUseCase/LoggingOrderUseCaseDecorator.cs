using Microsoft.Extensions.Logging;
using POS.Domains.Customer.UseCases.Orders.OrderUseCase.Dtos;

namespace POS.Domains.Customer.UseCases.Orders.OrderUseCase;
internal class LoggingOrderUseCaseDecorator
(
    IOrderUseCase next,
    ILogger<IOrderUseCase> logger
) : IOrderUseCase
{
    public async Task<OrderDto> GetOrderByIdAsync(Guid id)
    {
        try
        {
            logger.LogInformation("Getting order with id '{orderId}' ...", id);

            var order = await next.GetOrderByIdAsync(id);

            logger.LogInformation("Successfully got order with id '{orderId}'.", id);

            return order;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting order with id '{orderId}'.", id);
            throw;
        }
    }
}
