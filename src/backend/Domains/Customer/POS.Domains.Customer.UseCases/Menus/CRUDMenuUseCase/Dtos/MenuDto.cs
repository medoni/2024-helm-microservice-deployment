namespace POS.Domains.Customer.UseCases.Menus.CRUDMenuUseCase.Dtos;

/// <summary>
/// Dto for a Menu for the Customer.
/// </summary>
/// <param name="Id">Id of the menu.</param>
/// <param name="CreatedAt">Date and time when the Menu was created.</param>
/// <param name="LastChangedAt">Date and time when the Menu was last changed at.</param>
/// <param name="Sections">The sections of the menu.</param>
public record MenuDto
(
    Guid Id,
    DateTimeOffset CreatedAt,
    DateTimeOffset LastChangedAt,
    IReadOnlyList<MenuSectionDto> Sections
)
{
    /// <summary>
    /// True/false if the Menu is currently active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Date and time when the Menu was activated.
    /// </summary>
    public DateTimeOffset? ActivatedAt { get; set; }
}
