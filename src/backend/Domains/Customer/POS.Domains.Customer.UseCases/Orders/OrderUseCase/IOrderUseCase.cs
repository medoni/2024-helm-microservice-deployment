using POS.Domains.Customer.UseCases.Orders.OrderUseCase.Dtos;

namespace POS.Domains.Customer.UseCases.Orders.OrderUseCase;

/// <summary>
/// Definition to work with orders
/// </summary>
public interface IOrderUseCase
{
    /// <summary>
    /// Returns an order by its id.
    /// </summary>
    Task<OrderDto> GetOrderByIdAsync(Guid id);
}
