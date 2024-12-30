using POS.Domains.Payment.Service.Services.PaymentProvider.Paypal;
using POS.Shared.Testing;

namespace POS.Domains.Payment.Service.UnitTests.Services.PaymentProvider.Paypal;

[TestFixture]
[Category(TestCategories.Unit)]
public class PaypalPaymentOrderProviderTests
{
    private PaypalPaymentOrderProvider Sut { get; set; }

    [SetUp]
    public void SetUp()
    {
        Sut = new PaypalPaymentOrderProvider();
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
