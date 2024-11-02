namespace POS.Shared.Domain.Generic.Dtos;

public record MoneyDto(
    decimal Amount,
    string Currency
);
