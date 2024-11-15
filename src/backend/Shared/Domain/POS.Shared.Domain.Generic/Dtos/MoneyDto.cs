namespace POS.Shared.Domain.Generic.Dtos;

/// <summary>
/// Dto for <see cref="Money"/>
/// </summary>
/// <param name="Amount">Amount.</param>
/// <param name="Currency">Currency.</param>
public record MoneyDto(
    decimal Amount,
    string Currency
);
