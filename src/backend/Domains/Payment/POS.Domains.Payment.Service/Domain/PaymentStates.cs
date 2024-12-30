namespace POS.Domains.Payment.Service.Domain;
/// <summary>
/// States of the Payment
/// </summary>
public enum PaymentStates
{
    /// <summary>Created</summary>
    Created,

    /// <summary>Requested</summary>
    Requested,

    /// <summary>Approved</summary>
    Approved,

    /// <summary>Captured</summary>
    Captured,

    /// <summary>Canceled</summary>
    Canceled
}
