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
    Payed,

    /// <summary>
    /// Canceled
    /// </summary>
    Canceled
}
