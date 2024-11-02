using POS.Domains.Customer.Abstractions.Orders;
using POS.Domains.Customer.UseCases.Orders.OrderUseCase.Dtos;

namespace POS.Domains.Customer.UseCases.Orders.OrderUseCase.Mapper;
internal static class OrderPriceSummaryMapper
{
    public static OrderPriceSummaryDto ToDto(this OrderPriceSummary summary)
    {
        return new OrderPriceSummaryDto(
            summary.TotalItemPrice,
            summary.TotalPrice,
            summary.DeliveryCosts,
            summary.Discount
        );
    }
}
