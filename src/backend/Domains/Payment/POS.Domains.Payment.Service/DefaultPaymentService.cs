using Microsoft.Extensions.DependencyInjection;
using POS.Domains.Payment.Service.Domain;
using POS.Domains.Payment.Service.Dtos;
using POS.Domains.Payment.Service.Exceptions;
using POS.Domains.Payment.Service.Mapper;
using POS.Domains.Payment.Service.Processors;

namespace POS.Domains.Payment.Service;

internal class DefaultPaymentService(
    IServiceProvider serviceProvider,
    IPaymentRepository paymentRepository
) : IPaymentService
{
    public async Task<PaymentDetailsDto> RequestPaymentAsync(RequestPaymentDto dto)
    {
        var payment = await paymentRepository.TryGetByEntityIdAsync(dto.EntityType, dto.EntityId);
        if (payment != null) throw new PaymentStillInProgress(dto.EntityType, dto.EntityId);

        var processor = GetPaymentProcessor(dto.Provider);
        var providerState = await processor.RequestPaymentAsync(dto);

        var entity = new PaymentEntity
        {
            Id = Guid.NewGuid(),
            EntityId = dto.EntityId,
            EntityType = dto.EntityType,
            RequestedAt = dto.RequestedAt,
            Provider = dto.Provider,
            State = PaymentStates.Requested,
            ProviderState = providerState
        };

        await paymentRepository.AddOrUpdateAsync(entity);

        var detailsDto = entity.ToDetailsDto();
        return detailsDto;
    }

    public Task<PaymentDetailsDto> CapturePaymentAsync(CapturePaymentDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<PaymentDetailsDto> GetPaymentDetailsAsync(Guid paymentId)
    {
        throw new NotImplementedException();
    }

    private IPaymentProcessor GetPaymentProcessor(PaymentProviders providerType)
    {
        var provider = serviceProvider.GetRequiredKeyedService<IPaymentProcessor>(providerType);
        return provider;
    }
}
