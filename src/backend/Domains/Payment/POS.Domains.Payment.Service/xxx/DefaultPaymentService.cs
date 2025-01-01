using Microsoft.Extensions.DependencyInjection;
using POS.Domains.Payment.Service.Domain;
using POS.Domains.Payment.Service.Dtos;
using POS.Domains.Payment.Service.Events;
using POS.Domains.Payment.Service.Exceptions;
using POS.Domains.Payment.Service.Mapper;
using POS.Domains.Payment.Service.Processors;
using POS.Shared.Infrastructure.PubSub.Abstractions;

namespace POS.Domains.Payment.Service;

internal class DefaultPaymentService(
    IServiceProvider serviceProvider,
    IPaymentRepository paymentRepository,
    IEventPublisher eventPublisher
) : IPaymentService
{
    public async Task<PaymentDetailsDto> RequestPaymentAsync(RequestPaymentDto dto)
    {
        var paymentId = Guid.NewGuid();
        var payment = await paymentRepository.TryGetByEntityIdAsync(dto.EntityType, dto.EntityId);
        if (payment != null) throw new PaymentStillInProgress(dto.EntityType, dto.EntityId);

        var processor = GetPaymentProcessor(dto.Provider);
        var providerState = await processor.RequestPaymentAsync(paymentId, dto);

        var entity = new PaymentEntity
        {
            Id = paymentId,
            EntityId = dto.EntityId,
            EntityType = dto.EntityType,
            RequestedAt = dto.RequestedAt,
            Provider = dto.Provider,
            State = PaymentStates.Requested,
            ProviderState = providerState
        };

        await paymentRepository.AddAsync(entity);

        var detailsDto = entity.ToDetailsDto();
        return detailsDto;
    }

    public Task<PaymentDetailsDto> CapturePaymentAsync(CapturePaymentDto dto)
    {
        throw new NotImplementedException();
    }

    private async Task TryCapturePaymentAsync(Guid paymentId)
    {
        var paymentEntity = await paymentRepository.GetAsync(paymentId);
        if (paymentEntity.State == PaymentStates.Captured) return;
        if (paymentEntity.State == PaymentStates.Canceled) return;

        var paymentProcessor = GetPaymentProcessor(paymentEntity.Provider);
        var paymentProviderState = await paymentProcessor.CapturePaymentAsync(paymentEntity);

        paymentEntity.State = PaymentStates.Captured;
        paymentEntity.ProviderState = paymentProviderState;
        paymentEntity.PayedAt = paymentProviderState.CapturedAt;
        paymentEntity.CapturedAt = paymentProviderState.CapturedAt;

        await paymentRepository.UpdateAsync(paymentEntity);

        var evt = new PaymentSuccessfullyCapturedEvent(
            paymentEntity.Id,
            paymentEntity.EntityType,
            paymentEntity.EntityId,
            paymentEntity.RequestedAt,
            paymentProviderState.CapturedAt!.Value,
            paymentEntity.Provider
        );

        await eventPublisher.PublishAsync(evt);
    }

    private async Task TryUpdatePaymentApproval(Guid paymentId)
    {
        var paymentEntity = await paymentRepository.GetAsync(paymentId);
        if (paymentEntity.State == PaymentStates.Approved) return;
        if (paymentEntity.State == PaymentStates.Captured) return;
        if (paymentEntity.State == PaymentStates.Canceled) return;

        var paymentProcessor = GetPaymentProcessor(paymentEntity.Provider);
        var providerState = await paymentProcessor.CheckPaymentRequestAsync(paymentEntity);
    }

    public async Task<PaymentDetailsDto> GetPaymentDetailsAsync(Guid paymentId)
    {
        var payment = await paymentRepository.GetAsync(paymentId);
        return payment.ToDetailsDto();
    }

    private IPaymentProcessor GetPaymentProcessor(PaymentProviders providerType)
    {
        var provider = serviceProvider.GetRequiredKeyedService<IPaymentProcessor>(providerType);
        return provider;
    }

    public async Task OnSuccessfullyRequestedAsync(Guid paymentId)
    {
        await TryUpdatePaymentApproval(paymentId);
    }

    public Task OnCanceledAsync(Guid paymentId)
    {
        throw new NotImplementedException();
    }
}
