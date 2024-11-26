using POS.Shared.Domain.Generic.Dtos;

namespace POS.Shared.Domain.Generic.Mapper;
/// <summary>
/// Extension methods that converts <see cref="GrossNetPrice"/> to <see cref="GrossNetPriceDto"/>.
/// </summary>
public static class GrossNetPriceMapper
{
    /// <summary>
    /// Converts a <see cref="GrossNetPrice"/> to <see cref="GrossNetPriceDto"/>.
    /// </summary>
    public static GrossNetPriceDto ToDto(this GrossNetPrice value)
    => new()
    {
        Gross = value.Gross,
        Net = value.Net,
        Vat = value.Vat,
        Currency = value.Currency
    };

    /// <summary>
    /// Converts a <see cref="GrossNetPriceDto"/> to <see cref="GrossNetPrice"/>.
    /// </summary>
    public static GrossNetPrice ToDomain(this GrossNetPriceDto value)
    => new GrossNetPrice(
        value.Gross,
        value.Net,
        value.Vat,
        value.Currency
    );
}
