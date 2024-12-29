using POS.Domains.Payment.Service.Domain;
using POS.Shared.Domain.Events;

namespace POS.Domains.Payment.Service.Events;
/// <summary>
/// Event raised, when a requested payment was successfully captured.
/// </summary>
/// <param name="PaymentId">Id of the payment</param>
/// <param name="EntityType">Type of the entity</param>
/// <param name="EntityId">Id of the entity</param>
/// <param name="RequestedAt">Date and time when the payment was requested.</param>
/// <param name="PayedAt">Date and time when the payment was payed.</param>
/// <param name="CapturedAt">Date and time when payment was captured.</param>
/// <param name="PaymentProvider">The payment provider.</param>
public record PaymentSuccessfullyCapturedEvent(
    Guid PaymentId,
    EntityTypes EntityType,
    string EntityId,
    DateTimeOffset RequestedAt,
    DateTimeOffset PayedAt,
    DateTimeOffset CapturedAt,
    PaymentProviders PaymentProvider
) : IDomainEvent
{
}
