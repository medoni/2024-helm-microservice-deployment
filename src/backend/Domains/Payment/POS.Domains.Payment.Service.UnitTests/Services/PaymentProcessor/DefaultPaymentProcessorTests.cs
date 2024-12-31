using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using POS.Domains.Payment.Service.Domain.Models;
using POS.Domains.Payment.Service.Services.PaymentProcessor;
using POS.Domains.Payment.Service.Services.PaymentProvider;
using POS.Shared.Testing;

namespace POS.Domains.Payment.Service.UnitTests.Services.PaymentProcessor;

[TestFixture]
[Category(TestCategories.Unit)]
public class DefaultPaymentProcessorTests
{
    private DefaultPaymentProcessor Sut { get; set; }

    private IKeyedServiceProvider ServiceProviderMock { get; set; }

    private IPaymentProvider PaymentProviderMock { get; set; }
    private const PaymentProviderTypes DefaultPaymentProviderType = PaymentProviderTypes.Paypal;
    private const EntityTypes DefaultEntityType = EntityTypes.CustomerOrder;

    [SetUp]
    public void SetUp()
    {
        ServiceProviderMock = Substitute.For<IKeyedServiceProvider>();
        PaymentProviderMock = Substitute.For<IPaymentProvider>();

        Sut = new DefaultPaymentProcessor(
            ServiceProviderMock
        );

        ServiceProviderMock
            .GetRequiredKeyedService(typeof(IPaymentProvider), (DefaultPaymentProviderType, DefaultEntityType))
            .Returns(PaymentProviderMock);
    }

    #region RequestPaymentAsync

    [Test]
    public async Task RequestPaymentAsync_Should_Call_PaymentProvider()
    {
        // arrange
        var entityId = "abcdef1234";
        var requestedAt = DateTimeOffset.UtcNow;

        // act
        var result = await Sut.RequestPaymentAsync(
            DefaultPaymentProviderType,
            DefaultEntityType,
            entityId,
            requestedAt
        );

        // assert
        await PaymentProviderMock
            .Received()
            .RequestPaymentAsync(entityId, requestedAt);
    }

    [Test]
    public void RequestPaymentAsync_Should_Throw_Exception_When_Payment_Provider_Does_Not_Exists()
    {
        // arrange
        var entityId = "abcdef1234";
        var requestedAt = DateTimeOffset.UtcNow;
        var paymentProviderType = ((PaymentProviderTypes)(-1));
        var entityType = ((EntityTypes)(-1));

        ServiceProviderMock
            .GetRequiredKeyedService(typeof(IPaymentProvider), (paymentProviderType, entityType))
            .Throws(new InvalidOperationException());

        // act
        Assert.That(
            () => Sut.RequestPaymentAsync(
                paymentProviderType,
                entityType,
                entityId,
                requestedAt
            ),
            Throws.TypeOf<InvalidOperationException>()
        );
    }

    #endregion

    [Test]
    public void Test()
    {
        // arrange

        // act

        // assert
        Assert.Fail();
    }
}
