using POS.Domains.Customer.Abstractions.Carts;
using POS.Domains.Customer.Abstractions.Carts.Events;
using POS.Domains.Customer.Abstractions.Orders;
using POS.Domains.Customer.Domain.Menus;
using POS.Domains.Customer.Domain.Orders;
using POS.Shared.Domain;
using POS.Shared.Domain.Events;
using POS.Shared.Domain.Generic;
using POS.Shared.Domain.Generic.Mapper;

namespace POS.Domains.Customer.Domain.Carts;

/// <summary>
/// Aggregate root for carts
/// </summary>
public class Cart : AggregateRoot
{
    private readonly CartState _state;

    /// <inheritdoc/>
    public override TState GetCurrentState<TState>() => (dynamic)_state;

    /// <summary>
    /// Id of the cart.
    /// </summary>
    public override Guid Id => _state.Id;

    /// <summary>
    /// Date and time when the cart was created.
    /// </summary>
    public DateTimeOffset CreatedAt => _state.CreatedAt;

    /// <summary>
    /// Date and time when the cart was last changed at.
    /// </summary>
    public DateTimeOffset LastChangedAt
    {
        get => _state.LastChangedAt;
        private set => _state.LastChangedAt = value;
    }

    /// <summary>
    /// State of the cart.
    /// </summary>
    public CartStates State
    {
        get => _state.State;
        private set => _state.State = value;
    }

    /// <summary>
    /// The activate menu id when the cart was created.
    /// </summary>
    public Guid MenuId => _state.MenuId;

    /// <summary>
    /// Currency of the cart.
    /// </summary>
    public string Currency => _state.Currency;

    /// <summary>
    /// Items of the cart.
    /// </summary>
    public IReadOnlyList<CartItem> Items
    {
        get => _state.Items.ToArray();
    }

    /// <summary>
    /// Checkout info.
    /// </summary>
    public CartCheckoutInfo? CheckoutInfo
    {
        get => _state.CheckoutInfo;
        private set => _state.CheckoutInfo = value;
    }

    /// <summary>
    /// Creates a new <see cref="Cart"/>.
    /// </summary>
    public Cart(
        Guid id,
        DateTimeOffset createdAt,
        Menu menu
    )
    : this(new CartState(id, createdAt, menu.Id, menu.Currency)
    {
        State = CartStates.Created,
        LastChangedAt = createdAt
    })
    {
        if (!menu.IsActive) throw new ArgumentException("Cannot create cart of inactive menu.");

        Apply(new CartCreatedEvent(Id, createdAt, MenuId, menu.Currency));
    }

    /// <summary>
    /// Creates a new cart
    /// </summary>
    public Cart(CartState state)
    {
        _state = state;
    }

    /// <summary>
    /// Adds or updates an item
    /// </summary>
    /// <param name="changedAt">Date and time when the item was changed.</param>
    /// <param name="menu">The menu where the cart was created from.</param>
    /// <param name="menuItemId">Menu item to add or update.</param>
    /// <param name="newQuantity">New quantity of the item.</param>
    public void AddOrUpdateItem(
        DateTimeOffset changedAt,
        Menu menu,
        Guid menuItemId,
        int newQuantity
    )
    {
        EnsureCorrectMenu(MenuId, menu);
        EnsureCorrectMenu(menu, menuItemId, out var menuItem);
        if (newQuantity < 0) throw new ArgumentOutOfRangeException(nameof(newQuantity), "The value must be greater or equal zero.");

        LastChangedAt = changedAt;

        var item = _state.Items.FirstOrDefault(x => x.Id == menuItemId);
        if (newQuantity == 0)
        {
            if (item is null) return;

            _state.Items.Remove(item);
            Apply(new CartItemRemovedEvent(Id, item.Id, changedAt, item.UnitPrice.ToDto(), item.Quantity));
            return;
        }

        IDomainEvent domainEvent;
        if (item is null)
        {
            item = new CartItem(
                Guid.NewGuid(),
                changedAt,
                menuItemId,
                menuItem.Name,
                menuItem.Description,
                menuItem.Price
            );
            _state.Items.Add(item);
            domainEvent = new CartItemAddedEvent(Id, item.Id, menuItemId, changedAt, menuItem.Name, menuItem.Description, menuItem.Price.ToDto(), newQuantity);
        }
        else
        {
            domainEvent = new CartItemUpdatedEvent(Id, item.Id, changedAt, menuItem.Price.ToDto(), newQuantity);
        }

        item.Quantity = newQuantity;
        item.LastChangedAt = changedAt;
        Apply(domainEvent);
    }

    /// <summary>
    /// Calculates the total price of cart.
    /// </summary>
    public GrossNetPrice CalculateTotalPrice()
    {

        var total = Items.Aggregate(
            GrossNetPrice.CreateZero(Currency),
            (res, x) => res + (x.UnitPrice.Price * x.Quantity)
        );

        return total;
    }

    /// <summary>
    /// Checks out a cart
    /// </summary>
    public void Checkout(
        DateTimeOffset checkoutAt,
        Order order
    )
    {
        EnsureCorrectDateTime(LastChangedAt, checkoutAt, nameof(checkoutAt));

        if (order.State != OrderStates.Created) throw new ArgumentException($"Order must be in state '{OrderStates.Created}'.");
        if (order.OrderItems.Select(x => x.CartItemId).Except(Items.Select(x => x.Id)).Count() > 0) throw new ArgumentException($"The order with id '{order.Id}' has different items.");


        CheckoutInfo = new CartCheckoutInfo(checkoutAt, order.Id);
        State = CartStates.CheckedOut;
        LastChangedAt = checkoutAt;

        Apply(new CartCheckedOutEvent(Id, CheckoutInfo.CheckedOutAt, CheckoutInfo.OrderId));
    }

    #region Generic Ensure...

    private static void EnsureCorrectMenu(Guid activeMenuIdAtCreateTime, Menu menu)
    {
        if (activeMenuIdAtCreateTime != menu.Id) throw new ArgumentException("The menu isn't the one used to create the cart.");
        if (!menu.IsActive) throw new ArgumentException("The menu is not active any more.");
    }

    private static void EnsureCorrectMenu(Menu menu, Guid menuItemId, out MenuItem item)
    {
        item = menu.Sections
            .SelectMany(x => x.Items)
            .FirstOrDefault(x => x.Id == menuItemId)
            ?? throw new ArgumentException($"The menu item with id '{menuItemId}' was not found in the menu with id '{menu.Id}'.");
    }

    private static void EnsureCorrectDateTime(
        DateTimeOffset lastChangedAt,
        DateTimeOffset changingAt,
        string parameterName
    )
    {
        if (changingAt < lastChangedAt) throw new ArgumentException($"'{parameterName}' cannot be in the past.", parameterName);
    }

    #endregion
}
