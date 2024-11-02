using POS.Domains.Customer.Abstractions.Carts;
using POS.Domains.Customer.Abstractions.Carts.Events;
using POS.Persistence.PostgreSql.Data.Customer;

namespace POS.Persistence.PostgreSql.Repositories;
partial class PostgresCartRepository
{
    public Task ProcessUncommitedEventAsync(CartCreatedEvent evt)
    {
        var cart = new CartEntity(evt.CartId, evt.CreatedAt, evt.CreatedAt, evt.MenuId, evt.Currency);
        DbContext.Carts.Add(cart);
        return Task.CompletedTask;
    }

    public Task ProcessUncommitedEventAsync(CartItemAddedEvent evt)
    {
        var cartItem = new CartItemEntity(
            evt.ItemId,
            evt.CartId,
            evt.MenuItemId,
            evt.AddedAt,
            evt.AddedAt,
            evt.Name,
            evt.Description,
            evt.UnitPrice,
            evt.Quantity
        );
        DbContext.Set<CartItemEntity>().Add(cartItem);
        return Task.CompletedTask;
    }

    public async Task ProcessUncommitedEventAsync(CartItemUpdatedEvent evt)
    {
        var cartItem = await DbContext.Set<CartItemEntity>().FindAsync(evt.CartItemId)
            ?? throw new InvalidOperationException($"Cart item with id '{evt.CartItemId}' was not found.");

        cartItem.LastChangedAt = evt.UpdatedAt;
        cartItem.Quantity = evt.NewQuantity;
    }

    public async Task ProcessUncommitedEventAsync(CartItemRemovedEvent evt)
    {
        var cart = await DbContext.Carts.FindAsync(evt.CartId)
            ?? throw new InvalidOperationException($"Cart with id '{evt.CartId}' was not found.");

        var cartItem = await DbContext.Set<CartItemEntity>().FindAsync(evt.CartItemId)
            ?? throw new InvalidOperationException($"Cart item with id '{evt.CartItemId}' was not found.");

        DbContext.Set<CartItemEntity>().Remove(cartItem);
        cart.LastChangedAt = evt.RemovedAt;
    }

    public async Task ProcessUncommitedEventAsync(CartCheckedOutEvent evt)
    {
        var cart = await DbContext.Carts.FindAsync(evt.CartId)
            ?? throw new InvalidOperationException($"Cart with id '{evt.CartId}' was not found.");

        cart.State = CartStates.CheckedOut;
        cart.CheckoutInfo = new() { CartId = cart.Id, CheckedOutAt = evt.CheckedOutAt, OrderId = evt.OrderId };
        cart.LastChangedAt = evt.CheckedOutAt;
    }
}
