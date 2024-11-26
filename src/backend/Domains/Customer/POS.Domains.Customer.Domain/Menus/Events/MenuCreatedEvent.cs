using POS.Shared.Domain.Events;

namespace POS.Domains.Customer.Domain.Menus.Events;
/// <summary>
/// Event raised when a Menu was created.
/// </summary>
public record MenuCreatedEvent
(
    Guid MenuId,
    DateTimeOffset CreatedAt,
    string Currency,
    IReadOnlyList<MenuSection> Sections
) : IDomainEvent
{
}
