using POS.Shared.Domain.Events;

namespace POS.Domains.Customer.Abstractions.Carts.Events;

/// <summary>
/// Event raised when a new cart was created.
/// </summary>
public record CartCreatedEvent
(
    Guid CartId,
    DateTimeOffset CreatedAt,
    Guid MenuId,
    string Currency
) : IDomainEvent
{
}
