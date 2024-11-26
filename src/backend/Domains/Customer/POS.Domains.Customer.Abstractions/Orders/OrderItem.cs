using POS.Shared.Domain.Generic.Dtos;

namespace POS.Domains.Customer.Abstractions.Orders;

/// <summary>
/// Item within the order.
/// </summary>
public record OrderItem
(
    Guid ItemId,
    Guid CartItemId,
    string Name,
    string Description,
    PriceInfoDto UnitPrice,
    int Quantity,
    GrossNetPriceDto TotalPrice
)
{
}
