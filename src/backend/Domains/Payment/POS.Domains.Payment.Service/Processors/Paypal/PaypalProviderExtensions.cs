using POS.Shared.Domain.Generic.Dtos;
using System.Globalization;
using PaypalItem = PaypalServerSdk.Standard.Models.Item;
using PaypalLinks = System.Collections.Generic.List<PaypalServerSdk.Standard.Models.LinkDescription>;
using PaypalMoney = PaypalServerSdk.Standard.Models.Money;
using PosLinks = System.Collections.Generic.List<POS.Domains.Payment.Service.Domain.PaymentLinkDescription>;
using PosOrderItem = POS.Domains.Customer.Abstractions.Orders.OrderItem;

namespace POS.Domains.Payment.Service.Processors.Paypal;
internal static class PaypalProviderExtensions
{
    public static PaypalMoney ToPaypalMoney(this GrossNetPriceDto value)
    {
        return new PaypalMoney(value.Currency, value.Gross.ToString("c", CultureInfo.InvariantCulture));
    }

    public static PaypalItem ToPaypalItem(this PosOrderItem orderItem)
    {
        return new PaypalItem
        {
            Name = orderItem.Name,
            Description = orderItem.Description,
            Quantity = orderItem.Quantity.ToString("d", CultureInfo.InvariantCulture),
            UnitAmount = orderItem.UnitPrice.Price.ToPaypalMoney(),
            Tax = new PaypalMoney(
                orderItem.UnitPrice.Price.Currency,
                (orderItem.UnitPrice.Price.Vat * orderItem.Quantity).ToString("d", CultureInfo.InvariantCulture)
            )
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
                    "approve" => new Domain.PaymentLinkDescription(x.Href, Domain.PaymentLinkTypes.Approve, Domain.PaymentLinkMethods.GET),
                    _ => null
                }
            )
            .Where(x => x != null)
            .Cast<Domain.PaymentLinkDescription>()
            .ToList();
        ;
    }
}
