namespace POS.Shared.Domain.Generic.Dtos;

/// <summary>
/// Dto for <see cref="Money"/>
/// </summary>
public record MoneyDto
{
    /// <summary>
    /// Amount.
    /// </summary>
    public required decimal Amount { get; init; }

    /// <summary>
    /// Currency.
    /// </summary>
    public required string Currency { get; init; }

    /// <summary>
    /// Creates a new MoneyDto.
    /// </summary>
    public MoneyDto() { }

    /// <summary>
    /// Creates a new MoneyDto.
    /// </summary>
    public static MoneyDto Create(
        decimal amount,
        string currency
    )
    {
        return new()
        {
            Amount = amount,
            Currency = currency
        };
    }
}
