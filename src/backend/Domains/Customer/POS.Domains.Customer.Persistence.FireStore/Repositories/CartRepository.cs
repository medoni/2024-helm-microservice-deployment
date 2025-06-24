using Google.Cloud.Firestore;
using POS.Domains.Customer.Domain.Carts;
using POS.Domains.Customer.Persistence.Carts;
using POS.Domains.Customer.Persistence.FireStore.Entities;
using System.Text.Json;

namespace POS.Domains.Customer.Persistence.FireStore.Repositories;

internal class CartRepository : BaseFireStoreRepository<Cart, CartEntity>, ICartRepository
{
    public CartRepository(
        FirestoreDb firestoreDb,
        string collectionName
    ) : base(firestoreDb, collectionName)
    {
    }

    protected override Cart CreateAggregate(CartEntity entity)
    {
        var state = JsonSerializer.Deserialize<CartState>(entity.Payload)!;
        var aggregate = new Cart(state);
        return aggregate;
    }

    protected override CartEntity CreateFireStoreEntity(Cart aggregate)
    {
        var state = aggregate.GetCurrentState<CartState>();

        var entity = new CartEntity
        {
            Id = aggregate.Id.ToString(),
            CreatedAt = aggregate.CreatedAt.ToString("O"),
            Payload = JsonSerializer.Serialize(state)
        };
        return entity;
    }
}
