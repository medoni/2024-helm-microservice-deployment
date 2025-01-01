namespace POS.Domains.Payment.Api.Dtos;

/// <summary>
/// Dto for capturing a requested payment.
/// </summary>
public class CapturePaymentDto
{
    /// <summary>
    /// Id of the payment to capture.
    /// </summary>
    public required Guid PaymentId { get; init; }
}
