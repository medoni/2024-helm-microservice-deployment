using POS.Domains.Payment.Service.Domain;

namespace POS.Domains.Payment.Service.Exceptions;
/// <summary>
/// Exceptions that is thrown when a payment for an entity is still in progress.
/// </summary>
public class PaymentStillInProgress : Exception
{
    /// <summary>
    /// Creates a new <see cref="PaymentStillInProgress"/>
    /// </summary>
    public PaymentStillInProgress(EntityTypes type, string entityId)
    : base($"A payment for {type} with id '{entityId}' is still in progress.")
    {
    }
}
