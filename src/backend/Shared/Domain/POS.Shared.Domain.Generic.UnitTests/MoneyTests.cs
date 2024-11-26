using POS.Shared.Testing;

namespace POS.Shared.Domain.Generic.UnitTests;

[TestFixture]
[Category(TestCategories.Unit)]
public class MoneyTests
{
    [Test]
    public void Create_Should_Create_Correct_Value()
    {
        // act
        var money = Money.Create(42.43m, "EUR");

        // assert
        Assert.That(money.Amount, Is.EqualTo(42.43m));
        Assert.That(money.Currency, Is.EqualTo("EUR"));
    }

    [Test]
    public void Ctor_Should_Create_Correct_Value()
    {
        // act
        var money = new Money(42.43m, "EUR");

        // assert
        Assert.That(money.Amount, Is.EqualTo(42.43m));
        Assert.That(money.Currency, Is.EqualTo("EUR"));
    }

    [Test]
    public void Adding_Money_Should_Return_Correct_Result()
    {
        // arrange
        var a = new Money(7.10m, "EUR");
        var b = new Money(9.13m, "EUR");

        // act
        var result = a + b;

        // assert
        Assert.That(result.Amount, Is.EqualTo(16.23m));
        Assert.That(result.Currency, Is.EqualTo("EUR"));
    }

    [Test]
    public void Adding_Money_Should_Throw_Exception_When_Currency_Differ()
    {
        // arrange
        var a = new Money(7.10m, "EUR");
        var b = new Money(9.13m, "USD");

        // act
        Assert.That(() =>
        {
            return a + b;
        }, Throws.InvalidOperationException.With.Message.EqualTo("The currencies are not same."));
    }

    [Test]
    public void Substracting_Money_Should_Return_Correct_Result()
    {
        // arrange
        var a = new Money(17.44m, "EUR");
        var b = new Money(9.13m, "EUR");

        // act
        var result = a - b;

        // assert
        Assert.That(result.Amount, Is.EqualTo(8.31m));
        Assert.That(result.Currency, Is.EqualTo("EUR"));
    }

    [Test]
    public void Substracting_Money_Should_Throw_Exception_When_Currency_Differ()
    {
        // arrange
        var a = new Money(7.10m, "EUR");
        var b = new Money(9.13m, "USD");

        // act
        Assert.That(() =>
        {
            return a - b;
        }, Throws.InvalidOperationException.With.Message.EqualTo("The currencies are not same."));
    }

    [Test]
    public void Multiply_Money_Should_Return_Correct_Result()
    {
        // arrange
        var a = new Money(17.44m, "EUR");
        var b = 10;

        // act
        var result = a * b;

        // assert
        Assert.That(result.Amount, Is.EqualTo(174.4m));
        Assert.That(result.Currency, Is.EqualTo("EUR"));
    }

    [Test]
    public void Divide_Money_Should_Return_Correct_Result()
    {
        // arrange
        var a = new Money(20.55m, "EUR");
        var b = 5;

        // act
        var result = a / b;

        // assert
        Assert.That(result.Amount, Is.EqualTo(4.11m));
        Assert.That(result.Currency, Is.EqualTo("EUR"));
    }

    [Test]
    public void Equal_Should_Return_Correct_Result()
    {
        Assert.That(Money.Create(12.22m, "EUR").Equals(Money.Create(12.22m, "EUR")), Is.True);
        Assert.That(Money.Create(12.23m, "EUR").Equals(Money.Create(12.22m, "EUR")), Is.False);
        Assert.That(Money.Create(12.22m, "EUR").Equals(Money.Create(12.22m, "USD")), Is.False);
        Assert.That(Money.Create(12.22m, "EUR").Equals(null), Is.False);

        Assert.That(Money.Create(12.22m, "EUR") == Money.Create(12.22m, "EUR"), Is.True);
        Assert.That(Money.Create(12.23m, "EUR") == Money.Create(12.22m, "EUR"), Is.False);
        Assert.That(Money.Create(12.22m, "EUR") == Money.Create(12.22m, "USD"), Is.False);
    }
}
