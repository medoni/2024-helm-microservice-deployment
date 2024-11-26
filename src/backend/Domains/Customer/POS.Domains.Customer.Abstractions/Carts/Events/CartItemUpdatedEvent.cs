using POS.Shared.Domain.Events;
using POS.Shared.Domain.Generic.Dtos;

namespace POS.Domains.Customer.Abstractions.Carts.Events;

/// <summary>
/// Event raised when an existing cart item was updated.
/// </summary>
public record CartItemUpdatedEvent
(
    Guid CartId,
    Guid CartItemId,
    DateTimeOffset UpdatedAt,
    PriceInfoDto UnitPrice,
    int NewQuantity
) : IDomainEvent
{
}
