using POS.Shared.Domain.Events;

namespace POS.Domains.Customer.Abstractions.Carts.Events;
/// <summary>
/// Event raised when a cart was checked out.
/// </summary>
public record CartCheckedOutEvent
(
    Guid CartId,
    DateTimeOffset CheckedOutAt,
    Guid OrderId
) : IDomainEvent
{

}
