namespace POS.Domains.Payment.Service.Exceptions;
/// <summary>
/// Exception, raised when a payment capture was not completed.
/// </summary>
public class PaymentCaptureNotCompletedException : Exception
{
    /// <summary>
    /// Creates a new <see cref="PaymentCaptureNotCompletedException"/>.
    /// </summary>
    public PaymentCaptureNotCompletedException(Guid paymentId, string currentState)
    : base($"The capture of payment with id '{paymentId}' has not been completed. Current state: '{currentState}'")
    {

    }
}
