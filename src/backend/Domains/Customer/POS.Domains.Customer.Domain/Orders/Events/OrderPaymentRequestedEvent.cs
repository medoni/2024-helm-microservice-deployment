using POS.Domains.Customer.Domain.Orders.Models;
using POS.Shared.Domain.Events;

namespace POS.Domains.Customer.Domain.Orders.Events;
/// <summary>
/// Event raised when a payment for an order was successfully requested.
/// </summary>
public record OrderPaymentRequestedEvent(
    Guid OrderId,
    DateTimeOffset RequestedAt,
    PaymentInfo PaymentInfo
) : IDomainEvent
{
}
