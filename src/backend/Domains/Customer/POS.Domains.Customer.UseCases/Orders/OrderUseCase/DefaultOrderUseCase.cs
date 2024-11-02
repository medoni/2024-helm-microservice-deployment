using POS.Domains.Customer.Domain.Orders;
using POS.Domains.Customer.UseCases.Orders.OrderUseCase.Dtos;
using POS.Domains.Customer.UseCases.Orders.OrderUseCase.Mapper;
using POS.Shared.Persistence.UOW;

namespace POS.Domains.Customer.UseCases.Orders.OrderUseCase;
internal class DefaultOrderUseCase : IOrderUseCase
{
    private readonly UnitOfWorkFactory _uowFactory;

    public DefaultOrderUseCase(UnitOfWorkFactory uowFactory)
    {
        _uowFactory = uowFactory ?? throw new ArgumentNullException(nameof(uowFactory));
    }

    public async Task<OrderDto> GetOrderByIdAsync(Guid id)
    {
        var uow = _uowFactory();
        var order = await uow.GetAsync<Order>(id);

        return order.ToDto();
    }
}
