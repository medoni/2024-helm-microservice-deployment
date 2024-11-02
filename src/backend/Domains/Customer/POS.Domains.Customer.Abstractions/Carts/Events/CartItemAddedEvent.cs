using POS.Shared.Domain.Events;
using POS.Shared.Domain.Generic.Dtos;

namespace POS.Domains.Customer.Abstractions.Carts.Events;

/// <summary>
/// Event raised when the item was added.
/// </summary>
public record CartItemAddedEvent
(
    Guid CartId,
    Guid ItemId,
    Guid MenuItemId,
    DateTimeOffset AddedAt,
    string Name,
    string Description,
    PriceInfoDto UnitPrice,
    int Quantity
) : IDomainEvent
{
}
