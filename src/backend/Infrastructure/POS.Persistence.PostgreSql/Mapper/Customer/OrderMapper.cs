using POS.Domains.Customer.Domain.Orders;
using POS.Persistence.PostgreSql.Data.Customer;

namespace POS.Persistence.PostgreSql.Mapper.Customer;
internal static class OrderMapper
{
    public static OrderState ToState(this OrderEntity entity)
    {
        return new OrderState(
            entity.Id,
            entity.CreatedAt,
            entity.Items.ToDomain(),
            entity.PriceSummary.ToDomain()
        )
        {
            State = entity.State,
            LastChangedAt = entity.LastChangedAt
        };
    }
}
