using POS.Shared.Domain.Generic.Dtos;
using System.Diagnostics.CodeAnalysis;

namespace POS.Domains.Customer.UseCases.Orders.OrderUseCase.Dtos;

/// <summary>
/// Order price summary, including total price, VAT and discounts.
/// </summary>
public class OrderPriceSummaryDto
{
    /// <summary>
    /// Total item price
    /// </summary>
    public required GrossNetPriceDto TotalItemPrice { get; set; }

    /// <summary>
    /// Total price
    /// </summary>
    public required GrossNetPriceDto TotalPrice { get; set; }

    /// <summary>
    /// Delivery costs
    /// </summary>
    public required GrossNetPriceDto DeliveryCosts { get; set; }

    /// <summary>
    /// Discounts
    /// </summary>
    public required GrossNetPriceDto Discount { get; set; }

    /// <summary>
    /// Creates a new <see cref="OrderPriceSummaryDto"/>.
    /// </summary>
    public OrderPriceSummaryDto()
    {
    }

    /// <summary>
    /// Creates a new <see cref="OrderPriceSummaryDto"/>.
    /// </summary>
    [SetsRequiredMembers]
    public OrderPriceSummaryDto(
        GrossNetPriceDto totalItemPrice,
        GrossNetPriceDto totalPrice,
        GrossNetPriceDto deliveryCosts,
        GrossNetPriceDto discount
    )
    {
        TotalItemPrice = totalItemPrice ?? throw new ArgumentNullException(nameof(totalItemPrice));
        TotalPrice = totalPrice ?? throw new ArgumentNullException(nameof(totalPrice));
        DeliveryCosts = deliveryCosts ?? throw new ArgumentNullException(nameof(deliveryCosts));
        Discount = discount ?? throw new ArgumentNullException(nameof(discount));
    }
}
