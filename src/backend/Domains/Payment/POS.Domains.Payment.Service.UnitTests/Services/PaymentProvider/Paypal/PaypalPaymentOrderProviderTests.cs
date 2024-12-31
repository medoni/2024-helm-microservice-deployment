using Microsoft.Extensions.Options;
using NSubstitute;
using PaypalServerSdk.Standard.Models;
using POS.Domains.Payment.Service.Services.PaymentProvider.Paypal;
using POS.Shared.Persistence.UOW;
using POS.Shared.Testing;

namespace POS.Domains.Payment.Service.UnitTests.Services.PaymentProvider.Paypal;

[TestFixture]
[Category(TestCategories.Unit)]
public class PaypalPaymentOrderProviderTests
{
    private PaypalPaymentOrderProvider Sut { get; set; }

    private IUnitOfWork UnitOfWorkMock { get; set; }
    private IPaypalInternalApi PaypalInternalApiMock { get; set; }
    private PaypalPaymentSettings PaypalSettings { get; set; }

    [SetUp]
    public void SetUp()
    {
        UnitOfWorkMock = Substitute.For<IUnitOfWork>();
        PaypalInternalApiMock = Substitute.For<IPaypalInternalApi>();
        var uowFactory = new UnitOfWorkFactory(() => UnitOfWorkMock);
        PaypalSettings = new PaypalPaymentSettings
        {
            ApiAccessKey = "test",
            ApiSecretKey = "test",
            ReturnUrl = "http://example.com/payment/{id}/return",
            CancelUrl = "http://example.com/payment/{id}/cancel",
            Payee = new Payee
            {
                EmailAddress = "foo@example.com"
            }
        };

        Sut = new PaypalPaymentOrderProvider(
            uowFactory,
            PaypalInternalApiMock,
            Options.Create(PaypalSettings)
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
