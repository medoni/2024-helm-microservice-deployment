using Microsoft.Extensions.Options;
using NSubstitute;
using POS.Domains.Customer.Abstractions.Orders;
using POS.Domains.Customer.Domain.Orders;
using POS.Domains.Customer.Domain.Orders.Events;
using POS.Domains.Payment.Service.Domain;
using POS.Domains.Payment.Service.Domain.Events;
using POS.Domains.Payment.Service.Services.PaymentProvider.Paypal;
using POS.Shared.Domain.Generic.Dtos;
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

    private Order Order { get; set; }

    #region Order

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
            Payee = new()
            {
                EmailAddress = "foo@example.com"
            }
        };

        Sut = new PaypalPaymentOrderProvider(
            uowFactory,
            PaypalInternalApiMock,
            Options.Create(PaypalSettings)
        );

        Order = CreateOrder();
        UnitOfWorkMock
            .GetAsync<Order>(Order.Id)
            .Returns(Order);
    }

    private Order CreateOrder()
    {
        var orderItems = new List<OrderItem>()
        {
            new OrderItem(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Foo Item",
                "Foo Item Descr",
                new PriceInfoDto {
                    Price = GrossNetPriceDto.CreateByGross(MoneyDto.Create(42.43m, "EUR"), 7),
                    RegularyVatInPercent = 7
                },
                1,
                GrossNetPriceDto.CreateByGross(MoneyDto.Create(42.43m, "EUR"), 7)
            )
        };

        var orderState = new OrderState()
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTimeOffset.UtcNow,
            LastChangedAt = DateTimeOffset.UtcNow,
            State = OrderStates.Created,
            Items = orderItems,
            PriceSummary = new OrderPriceSummary(
                GrossNetPriceDto.CreateByGross(MoneyDto.Create(42.43m, "EUR"), 7),
                GrossNetPriceDto.CreateByGross(MoneyDto.Create(42.43m, "EUR"), 7),
                GrossNetPriceDto.CreateByGross(MoneyDto.Create(0, "EUR"), 7),
                GrossNetPriceDto.CreateByGross(MoneyDto.Create(0, "EUR"), 7)
            )
        };
        var order = new Order(orderState);
        return order;
    }

    #endregion

    [Test]
    public async Task RequestPaymentAsync_Should_Successfully_Request_Payment()
    {
        // arrange
        var requestAt = DateTimeOffset.UtcNow;

        PaymentAggregate? storedPayment = null;
        UnitOfWorkMock
            .When(x => x.Add<PaymentAggregate>(Arg.Any<PaymentAggregate>()))
            .Do(x => storedPayment = x.ArgAt<PaymentAggregate>(0));

        PaypalInternalApiMock
            .OrdersCreateAsync(Arg.Any<PaypalServerSdk.Standard.Models.OrderRequest>())
            .Returns(new PaypalServerSdk.Standard.Models.Order()
            {
                Id = "PaypalId1243",
                Status = PaypalServerSdk.Standard.Models.OrderStatus.Created,
                Payer = new()
                {
                    Name = new() { GivenName = "Jane", Surname = "Doe" },
                    EmailAddress = "jd@example.com"
                },
                Links = new()
                {
                    new() { Href = "example.com", Rel = "approve"}
                }
            });

        // act
        var result = await Sut.RequestPaymentAsync(Order.Id.ToString(), requestAt);

        // assert
        Assert.That(storedPayment, Is.Not.Null);
        Assert.That(
            storedPayment.GetUncommittedChanges().Select(x => x.GetType()),
            Is.EqualTo(new[] {
                typeof(PaymentCreatedEvent),
                typeof(PaymentRequestedEvent)
            })
        );

        Assert.That(
            Order.GetUncommittedChanges().Select(x => x.GetType()),
            Is.EqualTo(new[] {
                typeof(OrderPaymentRequestedEvent)
            })
        );
    }
}
