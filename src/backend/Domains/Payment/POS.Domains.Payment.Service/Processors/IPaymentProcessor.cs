using POS.Domains.Payment.Service.Domain;
using POS.Domains.Payment.Service.Dtos;

namespace POS.Domains.Payment.Service.Processors;
internal interface IPaymentProcessor
{
    Task<PaymentProviderState> RequestPaymentAsync(Guid paymentId, RequestPaymentDto dto);
    Task<PaymentProviderState> CheckPaymentRequestAsync(PaymentEntity paymentEntity);
    Task<PaymentProviderState> CapturePaymentAsync(PaymentEntity paymentEntity);
}
