using POS.Shared.Domain.Events;

namespace POS.Shared.Infrastructure.PubSub.Abstractions;

/// <summary>
/// Definition for publishing events in Event-Driven-Environment
/// </summary>
public interface IEventPublisher
{
    /// <summary>
    /// Publishes the given events.
    /// </summary>
    Task PublishAsync(params IDomainEvent[] events);

    /// <summary>
    /// Publishes the given events.
    /// </summary>
    Task PublishAsync(IEnumerable<IDomainEvent> events);
}
