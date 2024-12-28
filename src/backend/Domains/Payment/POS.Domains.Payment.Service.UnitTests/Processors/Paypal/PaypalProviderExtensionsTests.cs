using PaypalServerSdk.Standard.Models;
using POS.Domains.Customer.Abstractions.Orders;
using POS.Domains.Payment.Service.Domain;
using POS.Shared.Domain.Generic.Dtos;
using POS.Shared.Testing;
using POS.Shared.Testing.NUnit;
using Sut = POS.Domains.Payment.Service.Processors.Paypal.PaypalProviderExtensions;

namespace POS.Domains.Payment.Service.UnitTests.Processors.Paypal;

[TestFixture]
[Category(TestCategories.Unit)]
public class PaypalProviderExtensionsTests
{
    [TestCase(0, "EUR", "0.00 EUR")]
    [TestCase(42.43, "EUR", "42.43 EUR")]
    [TestCase(-42.43, "EUR", "-42.43 EUR")]
    public void ToPaypalMoney_Should_Return_Correct_Results(
        decimal money,
        string currency,
        string expectedOutput
    )
    {
        // arrange
        var grossNetValue = GrossNetPriceDto.CreateByGross(
            MoneyDto.Create(money, currency),
            7
        );

        // act
        var result = Sut.ToPaypalMoney(grossNetValue);

        // assert
        Assert.That($"{result.MValue} {result.CurrencyCode}", Is.EqualTo(expectedOutput));
    }

    [Test]
    public void CalculateTaxTotal_Should_Return_Correct_Items()
    {
        // arrange
        var orderItems = new List<OrderItem>()
        {
            // TODO: https://github.com/medoni/2024-helm-microservice-deployment/issues/19
            new OrderItem(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Item 1",
                "Description Item 1",
                new PriceInfoDto { Price = GrossNetPriceDto.CreateByGross(MoneyDto.Create(42.43m, "EUR"), 7), RegularyVatInPercent = 7 },
                1,
                GrossNetPriceDto.CreateByGross(MoneyDto.Create(42.43m, "EUR"), 7)
            ),
            new OrderItem(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Item 2",
                "Description Item 2",
                new PriceInfoDto { Price = GrossNetPriceDto.CreateByGross(MoneyDto.Create(23.23m, "EUR"), 7), RegularyVatInPercent = 7 },
                2,
                GrossNetPriceDto.CreateByGross(MoneyDto.Create(46.46m, "EUR"), 7)
            )
        };

        // act
        var result = Sut.CalculateTaxTotal(orderItems);

        // assert
        Assert.That($"{result.MValue} {result.CurrencyCode}", Is.EqualTo("0 EUR"));
    }


    [Test]
    public void ToPaypalItems_Should_Return_Correct_Items()
    {
        // arrange
        var orderItems = new List<OrderItem>()
        {
            // TODO: https://github.com/medoni/2024-helm-microservice-deployment/issues/19
            new OrderItem(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Item 1",
                "Description Item 1",
                new PriceInfoDto { Price = GrossNetPriceDto.CreateByGross(MoneyDto.Create(42.43m, "EUR"), 7), RegularyVatInPercent = 7 },
                1,
                GrossNetPriceDto.CreateByGross(MoneyDto.Create(42.43m, "EUR"), 7)
            ),
            new OrderItem(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Item 2",
                "Description Item 2",
                new PriceInfoDto { Price = GrossNetPriceDto.CreateByGross(MoneyDto.Create(23.23m, "EUR"), 7), RegularyVatInPercent = 7 },
                2,
                GrossNetPriceDto.CreateByGross(MoneyDto.Create(46.46m, "EUR"), 7)
            )
        };

        // act
        var result = Sut.ToPaypalItems(orderItems);

        // assert
        Assert.That(result.ToJson(), Is.EqualTo(
            """
            [
              {
                "Name": "Item 1",
                "UnitAmount": {
                  "CurrencyCode": "EUR",
                  "MValue": "42.43"
                },
                "Tax": {
                  "CurrencyCode": "EUR",
                  "MValue": "2.78"
                },
                "Quantity": "1",
                "Description": "Description Item 1",
                "Sku": null,
                "Url": null,
                "Category": null,
                "ImageUrl": null,
                "Upc": null
              },
              {
                "Name": "Item 2",
                "UnitAmount": {
                  "CurrencyCode": "EUR",
                  "MValue": "23.23"
                },
                "Tax": {
                  "CurrencyCode": "EUR",
                  "MValue": "3.04"
                },
                "Quantity": "2",
                "Description": "Description Item 2",
                "Sku": null,
                "Url": null,
                "Category": null,
                "ImageUrl": null,
                "Upc": null
              }
            ]
            """
        ));
    }


    [Test]
    public void ToPosLinks_Should_Return_Correct_Results()
    {
        // arrange
        var paypalLinks = new List<LinkDescription>()
        {
            new LinkDescription("http://example.com/approve", "approve", LinkHttpMethod.Get),
            new LinkDescription("http://example.com/capture", "capture", LinkHttpMethod.Post),
            new LinkDescription("http://example.com/foo", "foo", LinkHttpMethod.Get)
        };

        // act
        var result = Sut.ToPosLinks(paypalLinks);

        // assert
        Assert.That(result.ToJson(), Is.EqualTo(
            $$"""
            [
              {
                "Url": "http://example.com/approve",
                "Type": {{(int)PaymentLinkTypes.Approve}},
                "Method": {{(int)PaymentLinkMethods.GET}}
              }
            ]
            """));
    }
}
