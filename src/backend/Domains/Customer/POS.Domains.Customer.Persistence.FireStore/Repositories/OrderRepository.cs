using Google.Cloud.Firestore;
using POS.Domains.Customer.Domain.Orders;
using POS.Domains.Customer.Persistence.FireStore.Entities;
using POS.Domains.Customer.Persistence.Orders;

namespace POS.Domains.Customer.Persistence.FireStore.Repositories;

internal class OrderRepository : BaseFireStoreRepository<Order, OrderEntity>, IOrderRepository
{
    public OrderRepository(
        FirestoreDb firestoreDb,
        string collectionName
    ) : base(firestoreDb, collectionName)
    {
    }

    protected override OrderEntity CreateFireStoreEntity(Order aggregate)
    {
        var entity = new OrderEntity
        {
            Id = aggregate.Id.ToString(),
            CustomerId = aggregate.CustomerId.ToString(),
            TotalPrice = aggregate.TotalPrice,
            Status = aggregate.Status.ToString(),
            OrderDate = Timestamp.FromDateTime(aggregate.OrderDate.ToUniversalTime()),
            Items = aggregate.Items.Select(i => new OrderItemEntity
            {
                MenuItemId = i.MenuItemId.ToString(),
                Name = i.Name,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };

        return entity;
    }

    protected override Order CreateAggregate(OrderEntity entity)
    {
        var orderItems = entity.Items.Select(i => new OrderItem(
            Guid.Parse(i.MenuItemId),
            i.Name,
            i.Quantity,
            i.UnitPrice
        )).ToList();

        var orderStatus = Enum.Parse<OrderStatus>(entity.Status);
        
        var order = new Order(
            Guid.Parse(entity.Id),
            Guid.Parse(entity.CustomerId),
            orderItems,
            orderStatus,
            entity.OrderDate.ToDateTime()
        );

        return order;
    }
}