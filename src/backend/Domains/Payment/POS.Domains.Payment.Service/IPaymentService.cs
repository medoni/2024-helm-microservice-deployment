using POS.Domains.Payment.Service.Dtos;

namespace POS.Domains.Payment.Service;
/// <summary>
/// Definition for handling payments
/// </summary>
public interface IPaymentService
{
    /// <summary>
    /// Requests a payment for a given entity.
    /// </summary>
    Task<PaymentDetailsDto> RequestPaymentAsync(RequestPaymentDto dto);

    /// <summary>
    /// Trying to capture a requested payment that has already been paid.
    /// </summary>
    Task<PaymentDetailsDto> CapturePaymentAsync(CapturePaymentDto dto);

    /// <summary>
    /// Returns payment details for a given payment id.
    /// </summary>
    Task<PaymentDetailsDto> GetPaymentDetailsAsync(Guid paymentId);
}
