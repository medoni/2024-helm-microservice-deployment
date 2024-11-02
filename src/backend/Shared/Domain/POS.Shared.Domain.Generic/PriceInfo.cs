using POS.Shared.Domain.Generic.Dtos;

namespace POS.Shared.Domain.Generic;

/// <summary>
/// Price information.
/// </summary>
/// <param name="Price">Price in gross and net</param>
/// <param name="RegularyVatInPercent">Vat in percent.</param>
public record PriceInfo
(
    GrossNetPrice Price,
    decimal RegularyVatInPercent
)
{
    /// <summary>
    /// Currency
    /// </summary>
    public string Currency => Price.Currency;

    /// <summary>
    /// Creates a new <see cref="PriceInfoDto"/>;
    /// </summary>
    public static PriceInfo Create(
        GrossNetPrice price,
        decimal regularyVatInPercent
    )
    {
        if (regularyVatInPercent < 0) throw new ArgumentOutOfRangeException(nameof(regularyVatInPercent), "Vat in percent cannot be less than zero.");

        return new(price, regularyVatInPercent);
    }

    /// <summary>
    /// Creates a new price info by gross value
    /// </summary>
    public static PriceInfo CreateByGross(
        decimal grossValue,
        decimal vatInPercent,
        string currency
    )
    => Create(
        GrossNetPrice.CreateByGross(Money.Create(grossValue, currency), vatInPercent), vatInPercent
    );

    /// <summary>
    /// Creates a new price info by net value
    /// </summary>
    public static PriceInfo CreateByNet(
        decimal netValue,
        decimal vatInPercent,
        string currency
    )
    => Create(
        GrossNetPrice.CreateByGross(Money.Create(netValue, currency), vatInPercent), vatInPercent
    );

    #region math

    /// <inheritdoc/>
    public static PriceInfo operator *(PriceInfo a, decimal multiplier)
    {
        return new PriceInfo(
            a.Price * multiplier,
            a.RegularyVatInPercent
        );
    }

    /// <inheritdoc/>
    public static PriceInfo operator /(PriceInfo a, decimal divisor)
    {
        return a * (1 / divisor);
    }

    #endregion
}
