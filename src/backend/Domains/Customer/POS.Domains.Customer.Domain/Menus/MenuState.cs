namespace POS.Domains.Customer.Domain.Menus;

/// <summary>
/// State to represent a Menu
/// </summary>
/// <param name="MenuId">Id of the menu.</param>
/// <param name="Currency">The currency of the menu.</param>
/// <param name="CreatedAt">Date and time when the menu was created.</param>
public record MenuState(
    Guid MenuId,
    string Currency,
    DateTimeOffset CreatedAt
)
{
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
    public MenuState(
        Guid menuId,
        DateTimeOffset createdAt,
        string currency,
        List<MenuSection>? sections = null
    ) : this(menuId, currency, createdAt)
    {
        Sections = sections ?? new List<MenuSection>();
        LastChangedAt = createdAt;
    }

    /// <summary>
    /// Creates a new <see cref="MenuState"/>
    /// </summary>
    public MenuState(
        Guid menuId,
        DateTimeOffset createdAt,
        DateTimeOffset lastChangedAt,
        string currency,
        IReadOnlyList<MenuSection> sections,
        bool isActive, DateTimeOffset? activatedAt
    ) : this(menuId, currency, createdAt)
    {
        LastChangedAt = lastChangedAt;
        Sections = sections ?? throw new ArgumentNullException(nameof(sections));
        IsActive = isActive;
        ActivatedAt = activatedAt;
    }
}
