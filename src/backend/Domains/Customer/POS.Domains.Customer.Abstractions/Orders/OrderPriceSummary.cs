using POS.Shared.Domain.Generic.Dtos;

namespace POS.Domains.Customer.Abstractions.Orders;
/// <summary>
/// Order price summary, including total price, VAT and discounts.
/// </summary>
public record OrderPriceSummary
(
    GrossNetPriceDto TotalItemPrice,
    GrossNetPriceDto TotalPrice,
    GrossNetPriceDto DeliveryCosts,
    GrossNetPriceDto Discount
)
{
}
