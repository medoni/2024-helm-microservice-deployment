using System.Globalization;

namespace POS.Shared.Domain.Generic;

/// <summary>
/// Value object to represent a Money object which contains amount and currency.
/// </summary>
public class Money
{
    /// <summary>
    /// Amount.
    /// </summary>
    public decimal Amount { get; }

    /// <summary>
    /// Currency.
    /// </summary>
    public string Currency { get; }

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

    #region Equals

    /// <inheritdoc/>
    public bool Equals(Money? other)
    {
        if (Object.ReferenceEquals(other, null)) return false;

        if (this.Amount != other.Amount) return false;
        if (this.Currency != other.Currency) return false;

        return false;
    }

    /// <inheritdoc/>
    public static bool operator ==(Money a, Money b)
    {
        if (Object.ReferenceEquals(a, b)) return true;
        if (Object.ReferenceEquals(a, null)) return false;

        return a.Equals(b);
    }

    /// <inheritdoc/>
    public static bool operator !=(Money a, Money b)
    => !(a == b);

    /// <inheritdoc/>
    public override bool Equals(object? other)
    => Equals(other as Money);

    /// <inheritdoc/>
    public override int GetHashCode()
    => HashCode.Combine(Amount, Currency);

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
