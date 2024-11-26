using POS.Shared.Domain.Generic.Dtos;
using POS.Shared.Domain.Generic.Mapper;
using POS.Shared.Testing;

namespace POS.Shared.Domain.Generic.UnitTests.Mapper;

[TestFixture]
[Category(TestCategories.Unit)]
public class PriceInfoMapperTests
{
    [Test]
    public void ToDto_Should_Return_Correct_Result()
    {
        // arrange
        var value = PriceInfo.CreateByGross(12.13m, 7m, "EUR");

        // act
        var result = value.ToDto();

        // assert
        Assert.That(result.Price.Gross, Is.EqualTo(12.13m));
        Assert.That(result.RegularyVatInPercent, Is.EqualTo(7m));
    }

    [Test]
    public void ToDomain_Should_Return_Correct_Result()
    {
        // arrange
        var value = new PriceInfoDto
        {
            Price = GrossNetPriceDto.CreateByGross(MoneyDto.Create(12.13m, "EUR"), 7m),
            RegularyVatInPercent = 7m
        };

        // act
        var result = value.ToDomain();

        // assert
        Assert.That(result.Price.Gross, Is.EqualTo(12.13m));
        Assert.That(result.RegularyVatInPercent, Is.EqualTo(7m));
        Assert.That(result.Currency, Is.EqualTo("EUR"));
    }
}
