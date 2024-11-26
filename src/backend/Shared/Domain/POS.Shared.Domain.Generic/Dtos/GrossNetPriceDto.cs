using POS.Shared.Domain.Generic.Mapper;

namespace POS.Shared.Domain.Generic.Dtos;
/// <summary>
/// Dto that contains gross and net prices.
/// </summary>
public record GrossNetPriceDto
{
    /// <summary>
    /// Gross value.
    /// </summary>
    public required decimal Gross { get; init; }

    /// <summary>
    /// Net value.
    /// </summary>
    public required decimal Net { get; init; }

    /// <summary>
    /// Vat value.
    /// </summary>
    public required decimal Vat { get; init; }

    /// <summary>
    /// Currency.
    /// </summary>
    public required string Currency { get; init; }

    /// <summary>
    /// Creates a new <see cref="GrossNetPriceDto"/>.
    /// </summary>
    public GrossNetPriceDto()
    {
    }

    /// <summary>
    /// Creates a new <see cref="GrossNetPriceDto"/> by net value.
    /// </summary>
    public static GrossNetPriceDto CreateByNet(
        MoneyDto net,
        decimal vatInPercent
    )
    => GrossNetPrice.CreateByNet(net.ToDomain(), vatInPercent).ToDto();

    /// <summary>
    /// Creates a new <see cref="GrossNetPriceDto"/> by gross value.
    /// </summary>
    public static GrossNetPriceDto CreateByGross(
        MoneyDto gross,
        decimal vatInpercent
    )
    => GrossNetPrice.CreateByGross(gross.ToDomain(), vatInpercent).ToDto();
}
