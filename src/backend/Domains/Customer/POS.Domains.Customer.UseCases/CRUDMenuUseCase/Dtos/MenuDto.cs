namespace POS.Domains.Customer.UseCases.CRUDMenuUseCase.Dtos;
public record MenuDto
(
    Guid Id,
    DateTimeOffset CreatedAt,
    DateTimeOffset LastChangedAt,
    IReadOnlyList<MenuSectionDto> Sections
)
{
    public bool IsActive { get; set; }
    public DateTimeOffset? ActivatedAt { get; set; }
}
