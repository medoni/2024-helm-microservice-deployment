namespace POS.Domains.Customer.Domain.Menus.Dtos;

public record MenuEntity(
    Guid MenuId,
    DateTimeOffset CreatedAt
)
{
    public DateTimeOffset LastChangedAt { get; set; }

    public ICollection<MenuSectionEntity> Sections { get; } = new List<MenuSectionEntity>();

    public bool? IsActive { get; set; }
    public DateTimeOffset? ActivatedAt { get; set; }

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
