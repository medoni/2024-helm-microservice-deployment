namespace POS.Domains.Customer.Domain.Menus.Entities;

/// <summary>
/// Entity to represent a Menu
/// </summary>
/// <param name="MenuId">Id of the menu.</param>
/// <param name="CreatedAt">Date and time when the menu was created.</param>
public record MenuEntity(
    Guid MenuId,
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
    public ICollection<MenuSectionEntity> Sections { get; } = new List<MenuSectionEntity>();

    /// <summary>
    /// True when the Menu is currently active.
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Date and time when the Menu was activated.
    /// </summary>
    public DateTimeOffset? ActivatedAt { get; set; }

    /// <summary>
    /// Creates a new <see cref="MenuEntity"/>
    /// </summary>
    public MenuEntity(
        Guid menuId,
        DateTimeOffset createdAt,
        List<MenuSectionEntity>? sections = null
    ) : this(menuId, createdAt)
    {
        Sections = sections ?? new List<MenuSectionEntity>();
        LastChangedAt = createdAt;
    }
}
