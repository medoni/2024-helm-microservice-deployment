using System.Globalization;

namespace POS.Shared.Domain.Generic;

/// <summary>
/// Value object to represent a Money object which contains amount and currency.
/// </summary>
public class Money : ValueObject
{
    /// <summary>
    /// Amount.
    /// </summary>
    public decimal Amount { get; }

    /// <summary>
    /// Currency.
    /// </summary>
    public string Currency { get; }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    /// <summary>
    /// Creates a new <see cref="Money"/>
    /// </summary>
    public Money(
        decimal amount,
        string currency
    )
    {
        if (string.IsNullOrWhiteSpace(currency)) throw new ArgumentException("Currency cannot be empty.", nameof(Currency));

        Amount = amount;
        Currency = currency;
    }

    /// <summary>
    /// Creates a new <see cref="Money"/> object with given amount and currency.
    /// </summary>
    public static Money Create(decimal amount, string currency)
    => new Money(amount, currency);

    /// <summary>
    /// Creates a new <see cref="Money"/> object with given amount in EUR.
    /// </summary>
    public static Money CreateFromEUR(decimal amount)
    => new Money(amount, "EUR");

    #region maths

    /// <inheritdoc/>
    public static Money operator +(Money a, Money b)
    {
        EnsureSameCurrencies(a, b);

        return new Money(
            a.Amount + b.Amount,
            a.Currency
        );
    }

    /// <inheritdoc/>
    public static Money operator -(Money a, Money b)
    {
        EnsureSameCurrencies(a, b);

        return new Money(
            a.Amount - b.Amount,
            a.Currency
        );
    }

    /// <inheritdoc/>
    public static Money operator *(Money a, decimal multiplier)
    {
        return new Money(
            a.Amount * multiplier,
            a.Currency
        );
    }

    /// <inheritdoc/>
    public static Money operator /(Money a, decimal divisor)
    {
        return new Money(
            a.Amount / divisor,
            a.Currency
        );
    }

    #endregion

    private static void EnsureSameCurrencies(Money a, Money b)
    {
        if (a.Currency != b.Currency)
        {
            throw new InvalidOperationException("The currencies are not same.");
        }
    }

    /// <inheritdoc/>
    public override string ToString()
    => $"{Amount.ToString(CultureInfo.InvariantCulture)} {Currency}";
}
