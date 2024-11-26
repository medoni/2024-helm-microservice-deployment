namespace POS.Shared.Domain.Generic;

/// <summary>
/// Value object that contains gross, net and vat values.
/// </summary>
public class GrossNetPrice : ValueObject
{
    /// <summary>
    /// Gross amount.
    /// </summary>
    public decimal Gross { get; }

    /// <summary>
    /// Net amount.
    /// </summary>
    public decimal Net { get; }

    /// <summary>
    /// Vat amount.
    /// </summary>
    public decimal Vat { get; }

    /// <summary>
    /// Currency.
    /// </summary>
    public string Currency { get; }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Gross;
        yield return Net;
        yield return Vat;
        yield return Currency;
    }

    /// <summary>
    /// Creates a new <see cref="GrossNetPrice"/>.
    /// </summary>
    public GrossNetPrice(decimal gross, decimal net, decimal vat, string currency)
    {
        if (string.IsNullOrWhiteSpace(currency)) throw new ArgumentException($"'{nameof(currency)}' cannot be null or empty.", nameof(currency));

        Gross = gross;
        Net = net;
        Vat = vat;
        Currency = currency;
    }

    /// <summary>
    /// Creates a new <see cref="GrossNetPrice"/> by net value.
    /// </summary>
    public static GrossNetPrice CreateByNet(
        Money net,
        decimal vatInPercent
    )
    {
        var vat = net.Amount * vatInPercent / 100;
        return new GrossNetPrice(
            net.Amount + vat,
            net.Amount,
            vat,
            net.Currency
        );
    }

    /// <summary>
    /// Creates a new <see cref="GrossNetPrice"/> by gross value.
    /// </summary>
    public static GrossNetPrice CreateByGross(
        Money gross,
        decimal vatInPercent
    )
    {
        var net = gross.Amount * 100 / (100 + vatInPercent);
        return new GrossNetPrice(
            gross.Amount,
            net,
            gross.Amount - net,
            gross.Currency
        );
    }

    /// <summary>
    /// Creates an empty price.
    /// </summary>
    public static GrossNetPrice CreateZero(string currency)
    => new GrossNetPrice(
        0, 0, 0,
        currency
    );

    /// <summary>
    /// Creates a new <see cref="GrossNetPrice"/>.
    /// </summary>
    public static GrossNetPrice Create(
        decimal gross,
        decimal net,
        decimal vat,
        string currency
    )
    {
        if (string.IsNullOrWhiteSpace(currency)) throw new ArgumentException("Currency cannot be null or empty.");

        return new(gross, net, vat, currency);
    }

    #region math

    /// <inheritdoc/>
    public static GrossNetPrice operator *(GrossNetPrice a, decimal factor)
    {
        return new GrossNetPrice(
            a.Gross * factor,
            a.Net * factor,
            a.Vat * factor,
            a.Currency
        );
    }

    /// <inheritdoc/>
    public static GrossNetPrice operator /(GrossNetPrice a, decimal divisor)
    {
        return a * (1 / divisor);
    }

    /// <inheritdoc/>
    public static GrossNetPrice operator +(GrossNetPrice a, GrossNetPrice b)
    {
        if (a.Currency != b.Currency) throw new ArgumentException("Cannot add the values. Currency differs.");

        return new GrossNetPrice(
            a.Gross + b.Gross,
            a.Net + b.Net,
            a.Vat + b.Vat,
            a.Currency
        );
    }

    /// <inheritdoc/>
    public static GrossNetPrice operator -(GrossNetPrice a, GrossNetPrice b)
    {
        if (a.Currency != b.Currency) throw new ArgumentException("Cannot subtract the values. Currency differs.");

        return new GrossNetPrice(
            a.Gross - b.Gross,
            a.Net - b.Net,
            a.Vat - b.Vat,
            a.Currency
        );
    }

    #endregion
}
