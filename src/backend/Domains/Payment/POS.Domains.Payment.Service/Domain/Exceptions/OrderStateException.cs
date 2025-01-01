using POS.Domains.Payment.Service.Domain.Models;

namespace POS.Domains.Payment.Service.Domain.Exceptions;
/// <summary>
/// Exception, raised when the state of the payment is not expected.
/// </summary>
public class PaymentStateException : Exception
{
    /// <summary>
    /// Creates a new <see cref="PaymentStateException"/>.
    /// </summary>
    public PaymentStateException(
        Guid orderId,
        PaymentStates currentState,
        PaymentStates expectedState
    ) : base($"The payment with id {orderId} is an invalid state. Current state: '{currentState}'. Expected state: '{expectedState}'")
    {
    }
}
