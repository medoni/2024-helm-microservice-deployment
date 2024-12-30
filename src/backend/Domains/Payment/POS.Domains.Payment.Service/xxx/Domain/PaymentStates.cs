namespace POS.Domains.Payment.Service.Domain;
/// <summary>
/// States of the payment
/// </summary>
public enum PaymentStates
{
    /// <summary>
    /// Requested
    /// </summary>
    Requested,

    /// <summary>
    /// Payed
    /// </summary>
    Approved,

    /// <summary>
    /// Captured (successfully payed by the buyer)
    /// </summary>
    Captured,

    /// <summary>
    /// Canceled
    /// </summary>
    Canceled
}
