namespace POS.Domains.Customer.Domain.Menus;

/// <summary>
/// Menu Section.
/// </summary>
/// <param name="Id">Id of the section.</param>
/// <param name="Name">Name of the section.</param>
/// <param name="Items">List of Menu items.</param>
public record MenuSection
(
    Guid Id,
    string Name,
    IEnumerable<MenuItem> Items
)
{
}
