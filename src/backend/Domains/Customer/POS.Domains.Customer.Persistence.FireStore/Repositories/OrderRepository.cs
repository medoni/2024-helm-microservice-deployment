using Google.Cloud.Firestore;
using POS.Domains.Customer.Domain.Orders;
using POS.Domains.Customer.Persistence.FireStore.Entities;
using POS.Domains.Customer.Persistence.Orders;
using System.Text.Json;

namespace POS.Domains.Customer.Persistence.FireStore.Repositories;

internal class OrderRepository : BaseFireStoreRepository<Order, OrderEntity>, IOrderRepository
{
    public OrderRepository(
        FirestoreDb firestoreDb,
        string collectionName
    ) : base(firestoreDb, collectionName)
    {
    }

    protected override Order CreateAggregate(OrderEntity entity)
    {
        var state = JsonSerializer.Deserialize<OrderState>(entity.Payload)!;
        var aggregate = new Order(state);
        return aggregate;
    }

    protected override OrderEntity CreateFireStoreEntity(Order aggregate)
    {
        var state = aggregate.GetCurrentState<OrderState>();

        var entity = new OrderEntity
        {
            Id = aggregate.Id.ToString(),
            CreatedAt = aggregate.CreatedAt.ToString("O"),
            Payload = JsonSerializer.Serialize(state)
        };
        return entity;
    }
}
