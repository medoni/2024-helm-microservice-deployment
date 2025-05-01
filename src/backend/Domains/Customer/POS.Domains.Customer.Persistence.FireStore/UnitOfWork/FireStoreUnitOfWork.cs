using POS.Domains.Customer.Persistence.Carts;
using POS.Domains.Customer.Persistence.Menus;
using POS.Domains.Customer.Persistence.Orders;
using POS.Shared.Persistence.UOW;

namespace POS.Domains.Customer.Persistence.FireStore.UnitOfWork;

internal class FireStoreUnitOfWork : IUnitOfWork
{
    private readonly IMenuRespository _menuRespository;
    private readonly ICartRepository _cartRepository;
    private readonly IOrderRepository _orderRepository;

    public FireStoreUnitOfWork(
        IMenuRespository menuRepository,
        ICartRepository cartRepository,
        IOrderRepository orderRepository)
    {
        _menuRespository = menuRepository;
        _cartRepository = cartRepository;
        _orderRepository = orderRepository;
    }

    public Task<int> SaveChangesAsync()
    {
        // FireStore operations are atomic by nature and already saved when performed
        // No explicit save operation is required, return 0 affected records as changes were already persisted
        return Task.FromResult(0);
    }
}
