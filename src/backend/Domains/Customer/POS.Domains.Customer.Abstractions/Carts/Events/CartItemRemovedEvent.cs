using POS.Shared.Domain.Events;
using POS.Shared.Domain.Generic.Dtos;

namespace POS.Domains.Customer.Abstractions.Carts.Events;
/// <summary>
/// Event raised when a cart item was removed from the cart.
/// </summary>
public record CartItemRemovedEvent
(
    Guid CartId,
    Guid CartItemId,
    DateTimeOffset RemovedAt,
    PriceInfoDto UnitPrice,
    int oldQuantity
) : IDomainEvent
{
}
