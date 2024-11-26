using POS.Shared.Domain.Events;

namespace POS.Domains.Customer.Domain.Menus.Events;

/// <summary>
/// Event raised when a Menu was activated.
/// </summary>
public record MenuActivatedEvent
(
    Guid MenuId,
    DateTimeOffset ActivatedAt
) : IDomainEvent
{
}
