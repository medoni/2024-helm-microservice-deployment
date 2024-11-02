using System.Globalization;

namespace POS.Shared.Domain.Generic;

public class Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(
        decimal amount,
        string currency
    )
    {
        if (string.IsNullOrWhiteSpace(currency)) throw new ArgumentException("Currency cannot be empty.", nameof(Currency));

        Amount = amount;
        Currency = currency;
    }

    public static Money CreateFromEUR(decimal amount)
    => new Money(amount, "EUR");

    #region maths

    public static Money operator +(Money a, Money b)
    {
        EnsureSameCurrencies(a, b);

        return new Money(
            a.Amount + b.Amount,
            a.Currency
        );
    }

    public static Money operator -(Money a, Money b)
    {
        EnsureSameCurrencies(a, b);

        return new Money(
            a.Amount - b.Amount,
            a.Currency
        );
    }

    public static Money operator *(Money a, decimal multiplier)
    {
        return new Money(
            a.Amount * multiplier,
            a.Currency
        );
    }

    public static Money operator /(Money a, decimal divisor)
    {
        return new Money(
            a.Amount / divisor,
            a.Currency
        );
    }

    #endregion

    #region Equals

    public bool Equals(Money? other)
    {
        if (Object.ReferenceEquals(other, null)) return false;

        if (this.Amount != other.Amount) return false;
        if (this.Currency != other.Currency) return false;

        return false;
    }

    public static bool operator ==(Money a, Money b)
    {
        if (Object.ReferenceEquals(a, b)) return true;
        if (Object.ReferenceEquals(a, null)) return false;

        return a.Equals(b);
    }

    public static bool operator !=(Money a, Money b)
    => !(a == b);

    public override bool Equals(object? other)
    => Equals(other as Money);

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

    public override string ToString()
    => $"{Amount.ToString(CultureInfo.InvariantCulture)} {Currency}";
}
