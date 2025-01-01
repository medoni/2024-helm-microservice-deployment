using POS.Domains.Payment.Service.Domain.Events;
using POS.Persistence.PostgreSql.Data.Payment;

namespace POS.Persistence.PostgreSql.Repositories;
partial class PostgresPaymentRepository
{
    public Task ProcessUncommitedEventAsync(PaymentCreatedEvent evt)
    {
        var payment = new PaymentEntity(
            evt.Id,
            evt.CreatedAt,
            evt.OccurredAt,
            evt.NewState,
            evt.PaymentProvider,
            evt.EntityType,
            evt.EntityId,
            evt.Description,
            evt.TotalAmount
        );
        DbContext.Payments.Add(payment);
        return Task.CompletedTask;
    }

    public async Task ProcessUncommitedEventAsync(PaymentRequestedEvent evt)
    {
        var payment = await DbContext.Payments.FindAsync(evt.Id)
            ?? throw new InvalidOperationException($"Payment with id '{evt.Id}' was not found.");

        payment.LastChangedAt = evt.OccurredAt;
        payment.PaymentProvider = evt.PaymentProvider;
        payment.EntityType = evt.EntityType;
        payment.EntityId = evt.EntityId;
        payment.RequestedAt = evt.RequestedAt;
        payment.TotalAmount = evt.TotalAmount;
        payment.Links = evt.Links;
        payment.PaymentProviderPayload = evt.PaymentProviderPayload;
    }
}
