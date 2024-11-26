using POS.Shared.Domain.Generic.Dtos;
using POS.Shared.Domain.Generic.Mapper;
using POS.Shared.Testing;

namespace POS.Shared.Domain.Generic.UnitTests.Mapper;

[TestFixture]
[Category(TestCategories.Unit)]
public class MoneyMapperTests
{
    [Test]
    public void ToDto_Should_Return_Correct_Result()
    {
        // arrange
        var value = Money.Create(12.13m, "EUR");

        // act
        var result = value.ToDto();

        // assert
        Assert.That(result.Amount, Is.EqualTo(12.13m));
        Assert.That(result.Currency, Is.EqualTo("EUR"));
    }

    [Test]
    public void ToDomain_Should_Return_Correct_Result()
    {
        // arrange
        var value = MoneyDto.Create(12.13m, "EUR");

        // act
        var result = value.ToDomain();

        // assert
        Assert.That(result.Amount, Is.EqualTo(12.13m));
        Assert.That(result.Currency, Is.EqualTo("EUR"));
    }

}
