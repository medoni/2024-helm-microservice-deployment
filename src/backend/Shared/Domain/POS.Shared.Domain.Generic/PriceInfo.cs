using POS.Shared.Domain.Generic.Dtos;

namespace POS.Shared.Domain.Generic;

/// <summary>
/// Price information.
/// </summary>
public class PriceInfo : ValueObject
{
    /// <summary>
    /// Price in gross and net.
    /// </summary>
    public GrossNetPrice Price { get; }

    /// <summary>
    /// Vat in percent.
    /// </summary>
    public decimal RegularyVatInPercent { get; }

    /// <summary>
    /// Currency
    /// </summary>
    public string Currency => Price.Currency;

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Price;
        yield return RegularyVatInPercent;
    }

    /// <summary>
    /// Creates a new <see cref="PriceInfoDto"/>;
    /// </summary>
    public PriceInfo(
        GrossNetPrice price,
        decimal regularyVatInPercent
    )
    {
        if (regularyVatInPercent < 0) throw new ArgumentOutOfRangeException(nameof(regularyVatInPercent), "Vat in percent cannot be less than zero.");

        Price = price;
        RegularyVatInPercent = regularyVatInPercent;
    }

    /// <summary>
    /// Creates a new <see cref="PriceInfoDto"/>;
    /// </summary>
    public static PriceInfo Create(
        GrossNetPrice price,
        decimal regularyVatInPercent
    )
    {
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
