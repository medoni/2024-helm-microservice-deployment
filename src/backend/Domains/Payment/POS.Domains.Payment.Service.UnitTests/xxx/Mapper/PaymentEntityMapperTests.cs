using POS.Domains.Payment.Service.Domain;
using POS.Domains.Payment.Service.Mapper;
using POS.Shared.Domain.Generic.Dtos;
using POS.Shared.Testing;

namespace POS.Domains.Payment.Service.UnitTests.Mapper;

[TestFixture]
[Category(TestCategories.Unit)]
public class PaymentEntityMapperTests
{
    [Test]
    public void ToDetailsDto_Should_Return_Correct_Values()
    {
        // arrange
        var entity = new PaymentEntity()
        {
            Id = Guid.NewGuid(),
            EntityId = "ABCdef123",
            EntityType = EntityTypes.CustomerOrder,
            Provider = PaymentProviders.Paypal,
            PayedAt = DateTimeOffset.UtcNow,
            RequestedAt = DateTimeOffset.UtcNow,
            State = PaymentStates.Payed,
            ProviderState = new TestProviderState()
            {
                PaymentProviderId = "xyz1234",
                Amount = GrossNetPriceDto.CreateByGross(MoneyDto.Create(42.23m, "EUR"), 7),
                Description = "Lorem Ipsum",
                Links = new()
                {
                    new PaymentLinkDescription("http://example.com", PaymentLinkTypes.Approve, PaymentLinkMethods.GET)
                }
            }
        };

        // act
        var dto = entity.ToDetailsDto();

        // assert
        Assert.That(dto, Is.EqualTo(new
        {
            PaymentId = entity.Id,
            EntityId = entity.EntityId,
            EntityType = entity.EntityType,
            RequestedAt = entity.RequestedAt,
            State = entity.State,
            Provider = entity.Provider,
            Description = entity.ProviderState.Description,
            Amount = entity.ProviderState.Amount,
            Links = entity.ProviderState.Links,
            PayedAt = entity.PayedAt
        }).UsingJson());
    }

    private class TestProviderState : PaymentProviderState
    {

    }
}
