using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NSubstitute;
using PaypalServerSdk.Standard.Models;
using POS.Domains.Payment.Service.Domain.Models;
using POS.Domains.Payment.Service.Services.PaymentProvider;
using POS.Domains.Payment.Service.Services.PaymentProvider.Paypal;
using POS.Shared.Persistence.UOW;
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
        Services
            .AddTransient<IUnitOfWork>(_ => Substitute.For<IUnitOfWork>())
            .AddTransient<UnitOfWorkFactory>(svcp => () => svcp.GetRequiredService<IUnitOfWork>());

        Services.AddTransient<IOptions<PaypalPaymentSettings>>(_ => Options.Create(
            new PaypalPaymentSettings
            {
                ApiAccessKey = "test",
                ApiSecretKey = "test",
                ReturnUrl = "http://example.com/payment/{id}/return",
                CancelUrl = "http://example.com/payment/{id}/cancel",
                Payee = new Payee
                {
                    EmailAddress = "foo@example.com"
                }
            }
        ));
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
