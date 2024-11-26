using POS.Shared.Domain.Generic.Dtos;
using POS.Shared.Domain.Generic.Mapper;
using POS.Shared.Testing;

namespace POS.Shared.Domain.Generic.UnitTests.Mapper;

[TestFixture]
[Category(TestCategories.Unit)]
public class GrossNetPriceMapperTests
{
    [Test]
    public void ToDto_Should_Return_Correct_Result()
    {
        // arrange
        var value = GrossNetPrice.CreateByGross(Money.Create(12.13m, "EUR"), 7m);

        // act
        var result = value.ToDto();

        // assert
        Assert.That(result.Gross, Is.EqualTo(12.13m));
        Assert.That(result.Net, Is.EqualTo(11.33m).Within(0.01m));
        Assert.That(result.Vat, Is.EqualTo(0.79).Within(0.01m));
        Assert.That(result.Currency, Is.EqualTo("EUR"));
    }

    [Test]
    public void ToDomain_Should_Return_Correct_Result()
    {
        // arrange
        var value = GrossNetPriceDto.CreateByGross(MoneyDto.Create(12.13m, "EUR"), 7m);

        // act
        var result = value.ToDomain();

        // assert
        Assert.That(result.Gross, Is.EqualTo(12.13m));
        Assert.That(result.Net, Is.EqualTo(11.33m).Within(0.01m));
        Assert.That(result.Vat, Is.EqualTo(0.79).Within(0.01m));
        Assert.That(result.Currency, Is.EqualTo("EUR"));
    }
}
