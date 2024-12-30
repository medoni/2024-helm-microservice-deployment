using Microsoft.Extensions.DependencyInjection;
using POS.Domains.Payment.Service.Domain;
using POS.Domains.Payment.Service.Services.PaymentProvider;
using POS.Domains.Payment.Service.Services.PaymentProvider.Paypal;
using POS.Shared.Testing;

namespace POS.Domains.Payment.Service.UnitTests.Services.PaymentProvider.Paypal;

[TestFixture]
[Category(TestCategories.Unit)]
public class PaypalPaymentProviderStartupTests
{
    private IServiceCollection Services { get; set; }

    [SetUp]
    public void SetUp()
    {
        Services = new ServiceCollection();
    }

    [Test]
    public void AddPaypalPaymentSupport_Should_Add_Correct_Services()
    {
        // act
        Services.AddPaypalPaymentSupport();

        // assert
        var serviceProvider = Services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();

        Assert.That(scope.ServiceProvider.GetRequiredKeyedService<IPaymentProvider>((PaymentProviderTypes.Paypal, EntityTypes.CustomerOrder)), Is.Not.Null);
    }
}
