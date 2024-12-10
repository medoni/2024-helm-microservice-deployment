using POS.Domains.Customer.Domain.Orders;
using POS.Persistence.PostgreSql.Data.Customer;

namespace POS.Persistence.PostgreSql.Mapper.Customer;
internal static class OrderMapper
{
    public static OrderState ToState(this OrderEntity entity)
    {
        return new OrderState
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            LastChangedAt = entity.LastChangedAt,
            State = entity.State,
            Items = entity.Items.ToDomain(),
            PriceSummary = entity.PriceSummary.ToDomain()
        };
    }
}
