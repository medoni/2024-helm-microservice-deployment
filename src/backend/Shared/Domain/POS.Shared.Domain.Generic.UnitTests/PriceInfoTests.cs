using POS.Shared.Testing;

namespace POS.Shared.Domain.Generic.UnitTests;

[TestFixture]
[Category(TestCategories.Unit)]
public class PriceInfoTests
{
    private static GrossNetPrice GrossWith7Percent = GrossNetPrice.CreateByGross(Money.CreateFromEUR(12.13m), 7m);

    [Test]
    public void Create_Should_Create_With_Correct_Values()
    {
        // act
        var result = PriceInfo.Create(GrossWith7Percent, 7m);

        // assert
        Assert.That(result.Currency, Is.EqualTo("EUR"));
        Assert.That(result.RegularyVatInPercent, Is.EqualTo(7m));
        Assert.That(result.Price.Gross, Is.EqualTo(GrossWith7Percent.Gross));
    }

    [Test]
    public void Ctor_Should_Create_With_Correct_Values()
    {
        // act
        var result = new PriceInfo(GrossWith7Percent, 7m);

        // assert
        Assert.That(result.Currency, Is.EqualTo("EUR"));
        Assert.That(result.RegularyVatInPercent, Is.EqualTo(7m));
        Assert.That(result.Price.Gross, Is.EqualTo(GrossWith7Percent.Gross));
    }

    [Test]
    public void Multiply_Should_Return_Correct_Result()
    {
        // arrange
        var price = new PriceInfo(GrossWith7Percent, 7m);

        // act
        var result = price * 10;

        // assert
        Assert.That(result.Currency, Is.EqualTo("EUR"));
        Assert.That(result.RegularyVatInPercent, Is.EqualTo(7m));
        Assert.That(result.Price.Gross, Is.EqualTo(GrossWith7Percent.Gross * 10));
    }

    [Test]
    public void Divide_Should_Return_Correct_Result()
    {
        // arrange
        var price = new PriceInfo(GrossWith7Percent, 7m);

        // act
        var result = price / 10;

        // assert
        Assert.That(result.Currency, Is.EqualTo("EUR"));
        Assert.That(result.RegularyVatInPercent, Is.EqualTo(7m));
        Assert.That(result.Price.Gross, Is.EqualTo(GrossWith7Percent.Gross / 10));
    }
}
