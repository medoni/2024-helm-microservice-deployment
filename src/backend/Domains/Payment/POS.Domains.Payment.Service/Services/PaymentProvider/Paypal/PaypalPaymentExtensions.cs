using PaypalServerSdk.Standard.Models;
using POS.Domains.Payment.Service.Domain.Models;
using POS.Shared.Domain.Generic.Dtos;
using System.Globalization;
using PaypalAddress = PaypalServerSdk.Standard.Models.Address;
using PaypalItem = PaypalServerSdk.Standard.Models.Item;
using PaypalLinks = System.Collections.Generic.List<PaypalServerSdk.Standard.Models.LinkDescription>;
using PaypalMoney = PaypalServerSdk.Standard.Models.Money;
using PaypalPayer = PaypalServerSdk.Standard.Models.Payer;
using PosAddress = POS.Domains.Payment.Service.Domain.Models.PayerAddress;
using PosLinks = System.Collections.Generic.List<POS.Domains.Payment.Service.Domain.Models.PaymentLinkDescription>;
using PosOrderItem = POS.Domains.Customer.Abstractions.Orders.OrderItem;
using PosPayer = POS.Domains.Payment.Service.Domain.Models.Payer;

namespace POS.Domains.Payment.Service.Services.PaymentProvider.Paypal;
internal static class PaypalProviderExtensions
{
    public static decimal ToDecimal(this PaypalMoney money)
    {
        return decimal.Parse(money.MValue, CultureInfo.InvariantCulture);
    }

    public static PaypalMoney ToPaypalMoney(this decimal amount, string currencyCode)
    {
        return new PaypalMoney(currencyCode, amount.ToString("0.00", CultureInfo.InvariantCulture));
    }

    public static PaypalMoney ToPaypalMoney(this GrossNetPriceDto value)
    {
        return value.Net.ToPaypalMoney(value.Currency);
    }

    public static PaypalMoney CalculateTaxTotal(this IEnumerable<PaypalItem> items)
    {
        var vatTotal = items.Sum(x => x.Tax.ToDecimal() * int.Parse(x.Quantity));
        return vatTotal.ToPaypalMoney(items.First().Tax.CurrencyCode);
    }

    public static PaypalMoney CalculateItemTotal(this IEnumerable<PaypalItem> items)
    {
        var itemTotal = items.Sum(x => x.UnitAmount.ToDecimal() * int.Parse(x.Quantity));
        return itemTotal.ToPaypalMoney(items.First().Tax.CurrencyCode);
    }

    public static PaypalItem ToPaypalItem(this PosOrderItem orderItem)
    {
        return new PaypalItem
        {
            Name = orderItem.Name,
            Description = orderItem.Description,
            Quantity = orderItem.Quantity.ToString(CultureInfo.InvariantCulture),
            UnitAmount = orderItem.UnitPrice.Price.ToPaypalMoney(),
            Tax = (orderItem.UnitPrice.Price.Vat * orderItem.Quantity).ToPaypalMoney(orderItem.UnitPrice.Price.Currency)
        };
    }

    public static List<PaypalItem> ToPaypalItems(this IEnumerable<PosOrderItem> orderItems)
    {
        return orderItems.Select(ToPaypalItem).ToList();
    }

    public static PosLinks ToPosLinks(this PaypalLinks links)
    {
        return links.
            Select(x =>
                x.Rel switch
                {
                    "approve" => new PaymentLinkDescription(x.Href, PaymentLinkTypes.Approve, PaymentLinkMethods.GET),
                    _ => null
                }
            )
            .Where(x => x != null)
            .Cast<PaymentLinkDescription>()
            .ToList();
        ;
    }

    public static GrossNetPriceDto ToGrossNetPriceDto(this AmountWithBreakdown amount)
    {
        var gross = decimal.Parse(amount.MValue, CultureInfo.InvariantCulture);
        var vat = amount.Breakdown.TaxTotal.ToDecimal();
        var net = gross - vat;

        return new GrossNetPriceDto
        {
            Currency = amount.CurrencyCode,
            Gross = gross,
            Net = net,
            Vat = vat
        };
    }

    public static AmountWithBreakdown CalculateAmountWithBreakdown(
        this IEnumerable<PaypalItem> items,
        GrossNetPriceDto discount,
        GrossNetPriceDto deliveryCosts
    )
    {
        var itemTotal = items.CalculateItemTotal();
        var taxTotal = items.CalculateTaxTotal();

        var total =
            itemTotal.ToDecimal() +
            taxTotal.ToDecimal() +
            deliveryCosts.Net -
            deliveryCosts.Net;

        return new()
        {
            CurrencyCode = itemTotal.CurrencyCode,
            MValue = total.ToPaypalMoney(itemTotal.CurrencyCode).MValue,
            Breakdown = new AmountBreakdown
            {
                ItemTotal = itemTotal,
                TaxTotal = taxTotal,
                Discount = discount.ToPaypalMoney(),
                Shipping = deliveryCosts.ToPaypalMoney(),
            }
        };
    }

    public static PosPayer ToPosPayer(this PaypalPayer payer)
    {
        return new PosPayer
        {
            FirstName = payer.Name.GivenName,
            LastName = payer.Name.Surname,
            Email = payer.EmailAddress,
            Phone = payer.Phone?.PhoneNumber?.NationalNumber,
            Address = payer.Address?.ToPosAddress()
        };
    }

    public static PosAddress ToPosAddress(this PaypalAddress address)
    {
        return new PosAddress
        {
            AddressLine1 = address.AddressLine1,
            AddressLine2 = address.AddressLine2,
            Zip = address.PostalCode,
            City = address.AdminArea2,
            CountryCode = address.CountryCode
        };
    }
}
