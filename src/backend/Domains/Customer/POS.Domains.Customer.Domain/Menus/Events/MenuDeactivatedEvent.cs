using POS.Shared.Domain.Events;

namespace POS.Domains.Customer.Domain.Menus.Events;

/// <summary>
/// Event raised when a Menu was deactivated.
/// </summary>
public record MenuDeactivatedEvent
(
    Guid MenuId,
    DateTimeOffset DeactivatedAt
) : IDomainEvent
{
}
