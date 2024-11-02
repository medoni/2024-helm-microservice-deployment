using POS.Domains.Customer.Abstractions.Orders;
using POS.Persistence.PostgreSql.Data.Customer;

namespace POS.Persistence.PostgreSql.Mapper.Customer;
internal static class OrderPriceInformationMapper
{
    public static OrderPriceSummary ToDomain(this OrderPriceInformationEntity entity)
    {
        return new OrderPriceSummary(
            entity.TotalItemPrice,
            entity.TotalPrice,
            entity.DeliverCosts,
            entity.Discount
        );
    }

    public static OrderPriceInformationEntity ToEntity(this OrderPriceSummary domain, Guid orderId)
    {
        return new OrderPriceInformationEntity(
            orderId,
            domain.TotalItemPrice,
            domain.TotalPrice,
            domain.DeliveryCosts,
            domain.Discount
        );
    }
}
