using POS.Domains.Payment.Service.Domain;

namespace POS.Domains.Payment.Service.Exceptions;
/// <summary>
/// Exception that is thrown when a payment was not found.
/// </summary>
public class PaymentNotFoundException : Exception
{
    /// <summary>
    /// Creates a new <see cref="PaymentNotFoundException"/>.
    /// </summary>
    public PaymentNotFoundException(Guid paymentId)
    : base($"The payment with id '{paymentId}' was not found.")
    {

    }

    /// <summary>
    /// Creates a new <see cref="PaymentNotFoundException"/>.
    /// </summary>
    public PaymentNotFoundException(EntityTypes type, string entityId)
    : base($"The payment for {type} with id '{entityId}' was not found.")
    {

    }
}
