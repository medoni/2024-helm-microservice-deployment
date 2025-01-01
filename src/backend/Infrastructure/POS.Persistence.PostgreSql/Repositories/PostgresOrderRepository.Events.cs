using POS.Domains.Customer.Abstractions.Orders.Events;
using POS.Domains.Customer.Domain.Orders.Events;
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

    public async Task ProcessUncommitedEventAsync(OrderPaymentRequestedEvent evt)
    {
        var order = await DbContext.Orders.FindAsync(evt.OrderId)
            ?? throw new InvalidOperationException($"Order with id '{evt.OrderId}' was not found.");
        order.PaymentInfo = evt.PaymentInfo;
    }
}
