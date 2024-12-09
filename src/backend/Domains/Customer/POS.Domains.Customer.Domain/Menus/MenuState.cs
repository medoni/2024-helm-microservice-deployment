using System.Diagnostics.CodeAnalysis;

namespace POS.Domains.Customer.Domain.Menus;

/// <summary>
/// State to represent a Menu
/// </summary>
public record MenuState
{
    /// <summary>
    /// Id of the menu.
    /// </summary>
    public required Guid MenuId { get; init; }

    /// <summary>
    /// The currency of the menu.
    /// </summary>
    public required string Currency { get; init; }

    /// <summary>
    /// Date and time when the menu was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; init; }

    /// <summary>
    /// Date and time when the Menu was last changed at.
    /// </summary>
    public DateTimeOffset LastChangedAt { get; set; }

    /// <summary>
    /// List of sections of the Menu.
    /// </summary>
    public IReadOnlyList<MenuSection> Sections { get; set; } = new List<MenuSection>();

    /// <summary>
    /// True when the Menu is currently active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Date and time when the Menu was activated.
    /// </summary>
    public DateTimeOffset? ActivatedAt { get; set; }

    /// <summary>
    /// Creates a new <see cref="MenuState"/>
    /// </summary>
    public MenuState()
    {
    }

    /// <summary>
    /// Creates a new <see cref="MenuState"/>
    /// </summary>
    [SetsRequiredMembers]
    public MenuState(
        Guid menuId,
        DateTimeOffset createdAt,
        string currency,
        List<MenuSection>? sections = null
    )
    {
        MenuId = menuId;
        Currency = currency;
        CreatedAt = createdAt;
        Sections = sections ?? new List<MenuSection>();
        LastChangedAt = createdAt;
    }

    /// <summary>
    /// Creates a new <see cref="MenuState"/>
    /// </summary>
    [SetsRequiredMembers]
    public MenuState(
        Guid menuId,
        DateTimeOffset createdAt,
        DateTimeOffset lastChangedAt,
        string currency,
        IReadOnlyList<MenuSection> sections,
        bool isActive, DateTimeOffset? activatedAt
    )
    {
        MenuId = menuId;
        Currency = currency;
        CreatedAt = createdAt;
        LastChangedAt = lastChangedAt;
        Sections = sections ?? throw new ArgumentNullException(nameof(sections));
        IsActive = isActive;
        ActivatedAt = activatedAt;
    }
}
