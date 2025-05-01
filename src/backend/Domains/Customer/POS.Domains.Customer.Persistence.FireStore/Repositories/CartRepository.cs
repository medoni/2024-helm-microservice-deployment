using Google.Cloud.Firestore;
using POS.Domains.Customer.Domain.Carts;
using POS.Domains.Customer.Persistence.Carts;
using POS.Domains.Customer.Persistence.FireStore.Entities;

namespace POS.Domains.Customer.Persistence.FireStore.Repositories;

internal class CartRepository : BaseFireStoreRepository<Cart, CartEntity>, ICartRepository
{
    public CartRepository(
        FirestoreDb firestoreDb,
        string collectionName
    ) : base(firestoreDb, collectionName)
    {
    }

    protected override CartEntity CreateFireStoreEntity(Cart aggregate)
    {
        var entity = new CartEntity
        {
            Id = aggregate.Id.ToString(),
            CustomerId = aggregate.CustomerId.ToString(),
            TotalPrice = aggregate.TotalPrice,
            Items = aggregate.Items.Select(i => new CartItemEntity
            {
                MenuItemId = i.MenuItemId.ToString(),
                Name = i.Name,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };

        return entity;
    }

    protected override Cart CreateAggregate(CartEntity entity)
    {
        var cartItems = entity.Items.Select(i => new CartItem(
            Guid.Parse(i.MenuItemId),
            i.Name,
            i.Quantity,
            i.UnitPrice
        )).ToList();

        var cart = new Cart(
            Guid.Parse(entity.Id),
            Guid.Parse(entity.CustomerId),
            cartItems
        );

        return cart;
    }
}