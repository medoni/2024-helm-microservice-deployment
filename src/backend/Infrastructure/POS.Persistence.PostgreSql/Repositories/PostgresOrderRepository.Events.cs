using POS.Domains.Customer.Abstractions.Orders.Events;
using POS.Persistence.PostgreSql.Data.Customer;
using POS.Persistence.PostgreSql.Mapper.Customer;

namespace POS.Persistence.PostgreSql.Repositories;

partial class PostgresOrderRepository
{
    public Task ProcessUncommitedEventAsync(OrderCreatedByCheckoutEvent evt)
    {
        var order = new OrderEntity(
            evt.OrderId,
            evt.CreatedAt,
            evt.CreatedAt,
            evt.State,
            evt.Items.ToEntity(evt.OrderId),
            evt.PriceSummary.ToEntity(evt.OrderId)
        );

        DbContext.Orders.Add(order);
        return Task.CompletedTask;
    }
}
