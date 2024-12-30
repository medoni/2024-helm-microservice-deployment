using NSubstitute;
using POS.Domains.Payment.Service.Services.PaymentProcessor;
using POS.Shared.Testing;

namespace POS.Domains.Payment.Service.UnitTests.Services.PaymentProcessor;

[TestFixture]
[Category(TestCategories.Unit)]
public class DefaultPaymentProcessorTests
{
    private DefaultPaymentProcessor Sut { get; set; }

    private IServiceProvider ServiceProviderMock { get; set; }

    [SetUp]
    public void SetUp()
    {
        ServiceProviderMock = Substitute.For<IServiceProvider>();

        Sut = new DefaultPaymentProcessor(
            ServiceProviderMock
        );
    }

    [Test]
    public void Test()
    {
        // arrange

        // act

        // assert
        Assert.Fail();
    }
}
