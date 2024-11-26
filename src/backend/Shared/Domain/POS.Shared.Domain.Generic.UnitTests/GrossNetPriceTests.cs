using POS.Shared.Testing;

namespace POS.Shared.Domain.Generic.UnitTests;

[TestFixture]
[Category(TestCategories.Unit)]
public class GrossNetPriceTests
{
    [Test]
    public void Create_Should_Create_Correct_Value()
    {
        // act
        var result = GrossNetPrice.Create(12.13m, 13.14m, 14.15m, "EUR");

        // assert
        Assert.That(result.Gross, Is.EqualTo(12.13m));
        Assert.That(result.Net, Is.EqualTo(13.14m));
        Assert.That(result.Vat, Is.EqualTo(14.15m));
        Assert.That(result.Currency, Is.EqualTo("EUR"));
    }

    [Test]
    public void CreateByGross_Should_Create_Correct_Value()
    {
        // act
        var result = GrossNetPrice.CreateByGross(Money.CreateFromEUR(12.13m), 7m);

        // assert
        Assert.That(result.Gross, Is.EqualTo(12.13m));
        Assert.That(result.Net, Is.EqualTo(11.34).Within(0.01m));
        Assert.That(result.Vat, Is.EqualTo(0.79).Within(0.01m));
        Assert.That(result.Currency, Is.EqualTo("EUR"));
    }

    [Test]
    public void CreateByNet_Should_Create_Correct_Value()
    {
        // act
        var result = GrossNetPrice.CreateByNet(Money.CreateFromEUR(12.13m), 7m);

        // assert
        Assert.That(result.Net, Is.EqualTo(12.13m));
        Assert.That(result.Gross, Is.EqualTo(12.97m).Within(0.01m));
        Assert.That(result.Vat, Is.EqualTo(0.84).Within(0.01m));
        Assert.That(result.Currency, Is.EqualTo("EUR"));
    }

    [Test]
    public void CreateByZero_Should_Create_Correct_Value()
    {
        // act
        var result = GrossNetPrice.CreateZero("EUR");

        // assert
        Assert.That(result.Net, Is.EqualTo(0));
        Assert.That(result.Gross, Is.EqualTo(0));
        Assert.That(result.Vat, Is.EqualTo(0));
        Assert.That(result.Currency, Is.EqualTo("EUR"));
    }

    [Test]
    public void Ctor_Should_Create_Correct_Value()
    {
        // act
        var result = new GrossNetPrice(12.13m, 13.14m, 14.15m, "EUR");

        // assert
        Assert.That(result.Gross, Is.EqualTo(12.13m));
        Assert.That(result.Net, Is.EqualTo(13.14m));
        Assert.That(result.Vat, Is.EqualTo(14.15m));
        Assert.That(result.Currency, Is.EqualTo("EUR"));
    }

    [Test]
    public void Add_Should_Create_Correct_Value()
    {
        // arrange
        var a = GrossNetPrice.Create(6.7m, 7.8m, 8.9m, "EUR");
        var b = GrossNetPrice.Create(12.13m, 13.14m, 14.15m, "EUR");

        // act
        var result = a + b;

        // assert
        Assert.That(result.Gross, Is.EqualTo(18.83m));
        Assert.That(result.Net, Is.EqualTo(20.94m));
        Assert.That(result.Vat, Is.EqualTo(23.05m));
        Assert.That(result.Currency, Is.EqualTo("EUR"));
    }

    [Test]
    public void Add_With_Different_Currency_Should_Throw_Exception()
    {
        // arrange
        var a = GrossNetPrice.Create(6.7m, 7.8m, 8.9m, "EUR");
        var b = GrossNetPrice.Create(12.13m, 13.14m, 14.15m, "USD");

        // act
        Assert.That(() =>
        {
            return a + b;
        }, Throws.ArgumentException.With.Message.EqualTo("Cannot add the values. Currency differs."));
    }

    [Test]
    public void Substract_Should_Create_Correct_Value()
    {
        // arrange
        var a = GrossNetPrice.Create(12.13m, 13.14m, 14.15m, "EUR");
        var b = GrossNetPrice.Create(6.7m, 7.8m, 8.9m, "EUR");

        // act
        var result = a - b;

        // assert
        Assert.That(result.Gross, Is.EqualTo(5.43m));
        Assert.That(result.Net, Is.EqualTo(5.34m));
        Assert.That(result.Vat, Is.EqualTo(5.25m));
        Assert.That(result.Currency, Is.EqualTo("EUR"));
    }

    [Test]
    public void Substract_With_Different_Currency_Should_Throw_Exception()
    {
        // arrange
        var a = GrossNetPrice.Create(12.13m, 13.14m, 14.15m, "EUR");
        var b = GrossNetPrice.Create(6.7m, 7.8m, 8.9m, "USD");

        // act
        Assert.That(() =>
        {
            return a - b;
        }, Throws.ArgumentException.With.Message.EqualTo("Cannot subtract the values. Currency differs."));
    }

    [Test]
    public void Multiply_Should_Create_Correct_Value()
    {
        // arrange
        var a = GrossNetPrice.Create(12.13m, 13.14m, 14.15m, "EUR");

        // act
        var result = a * 10;

        // assert
        Assert.That(result.Gross, Is.EqualTo(121.3m));
        Assert.That(result.Net, Is.EqualTo(131.4m));
        Assert.That(result.Vat, Is.EqualTo(141.5m));
        Assert.That(result.Currency, Is.EqualTo("EUR"));
    }

    [Test]
    public void Divide_Should_Create_Correct_Value()
    {
        // arrange
        var a = GrossNetPrice.Create(12.13m, 13.14m, 14.15m, "EUR");

        // act
        var result = a / 10;

        // assert
        Assert.That(result.Gross, Is.EqualTo(1.213m));
        Assert.That(result.Net, Is.EqualTo(1.314m));
        Assert.That(result.Vat, Is.EqualTo(1.415m));
        Assert.That(result.Currency, Is.EqualTo("EUR"));
    }
}
