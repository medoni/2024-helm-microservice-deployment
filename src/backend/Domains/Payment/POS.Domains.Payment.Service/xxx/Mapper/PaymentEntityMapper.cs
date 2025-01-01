using POS.Domains.Payment.Service.Domain;
using POS.Domains.Payment.Service.Dtos;

namespace POS.Domains.Payment.Service.Mapper;
internal static class PaymentEntityMapper
{
    public static PaymentDetailsDto ToDetailsDto(this PaymentEntity entity)
    => new PaymentDetailsDto
    {
        PaymentId = entity.Id,
        EntityId = entity.EntityId,
        EntityType = entity.EntityType,
        RequestedAt = entity.RequestedAt,
        State = entity.State,
        Provider = entity.Provider,
        Description = entity.ProviderState.Description,
        Amount = entity.ProviderState.Amount,
        Links = entity.ProviderState.Links,
        PayedAt = entity.PayedAt
    };
}
