using POS.Domains.Payment.Service.Domain;
using POS.Persistence.PostgreSql.Data.Payment;

namespace POS.Persistence.PostgreSql.Mapper.Payment;
internal static class PaymentMapper
{
    public static PaymentAggregateState ToState(this PaymentEntity entity)
    {
        return new PaymentAggregateState
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            PaymentProvider = entity.PaymentProvider,
            EntityType = entity.EntityType,
            EntityId = entity.EntityId,
            State = entity.State,
            Description = entity.Description,
            Links = entity.Links.ToList(),
            TotalAmount = entity.TotalAmount,
            PaymentProviderPayload = entity.PaymentProviderPayload,
            RequestedAt = entity.RequestedAt
        };
    }
}
