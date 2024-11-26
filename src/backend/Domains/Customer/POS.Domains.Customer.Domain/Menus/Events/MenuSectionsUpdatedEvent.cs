using POS.Shared.Domain.Events;

namespace POS.Domains.Customer.Domain.Menus.Events;
/// <summary>
/// Event raised when menu sections where updated.
/// </summary>
public record MenuSectionsUpdatedEvent
(
    Guid MenuId,
    DateTimeOffset UpdatedAt,
    IReadOnlyList<MenuSection> NewSections
) : IDomainEvent
{
}
