namespace POS.Shared.Domain.Generic.Dtos;

/// <summary>
/// Price information.
/// </summary>
public record PriceInfoDto
{
    /// <summary>
    /// Price in gross and net.
    /// </summary>
    public required GrossNetPriceDto Price { get; init; }

    /// <summary>
    /// Vat in percent.
    /// </summary>
    public required decimal RegularyVatInPercent { get; init; }

    /// <summary>
    /// Creates a new <see cref="PriceInfoDto"/>;
    /// </summary>
    public PriceInfoDto() { }
}
