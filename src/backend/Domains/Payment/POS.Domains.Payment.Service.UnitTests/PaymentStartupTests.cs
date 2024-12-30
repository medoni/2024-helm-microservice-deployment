using Microsoft.Extensions.DependencyInjection;
using POS.Domains.Payment.Service.Services.PaymentProcessor;
using POS.Shared.Testing;

namespace POS.Domains.Payment.Service.UnitTests;

[TestFixture]
[Category(TestCategories.Unit)]
public class PaymentStartupTests
{
    private IServiceCollection Services { get; set; }

    [SetUp]
    public void SetUp()
    {
        Services = new ServiceCollection();
    }

    [Test]
    public void AddPaymentSupport_Should_Add_Correct_Services()
    {
        // act
        Services.AddPaymentSupport();

        // assert
        var serviceProvider = Services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();

        Assert.That(scope.ServiceProvider.GetRequiredService<IPaymentProcessor>(), Is.Not.Null);
    }
}
