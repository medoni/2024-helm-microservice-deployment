using POS.Shared.Domain.Events;
using POS.Shared.Infrastructure.PubSub.Abstractions;

namespace PizzaService.AspNetCore.Services.EventPublisher;

internal class NullEventPublisher : IEventPublisher
{
    public Task PublishAsync(params IDomainEvent[] events)
    {
        return Task.CompletedTask;
    }

    public Task PublishAsync(IEnumerable<IDomainEvent> events)
    {
        return Task.CompletedTask;
    }
}
