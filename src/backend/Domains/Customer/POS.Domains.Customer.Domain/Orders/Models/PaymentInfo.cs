namespace POS.Domains.Customer.Domain.Orders.Models;
/// <summary>
/// Information about the payment for the customer order
/// </summary>
public class PaymentInfo
{
    /// <summary>
    /// Id of the payment.
    /// </summary>
    public required Guid PaymentId { get; init; }

    /// <summary>
    /// Date and time when the payment was requested.
    /// </summary>
    public DateTimeOffset RequestedAt { get; init; }

    /// <summary>
    /// Date and time when the payment was successfully completed.
    /// </summary>
    public DateTimeOffset? PayedAt { get; init; }
}
