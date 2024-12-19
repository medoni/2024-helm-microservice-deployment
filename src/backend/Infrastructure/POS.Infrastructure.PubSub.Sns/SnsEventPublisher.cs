using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.Extensions.Logging;
using POS.Infrastructure.PubSub.Sns.Configurations;
using POS.Shared.Domain.Events;
using POS.Shared.Infrastructure.PubSub.Abstractions;
using System.Text.Json;

namespace POS.Infrastructure.PubSub.Sns;

internal class SnsEventPublisher(
    ILogger<SnsEventPublisher> logger,
    IAmazonSimpleNotificationService snsClient,
    SnsEventPublisherSettings settings
) : IEventPublisher
{
    public async Task PublishAsync(params IDomainEvent[] events)
    {
        foreach (var evt in events)
        {
            await PublishAsync(evt);
        }
    }

    public async Task PublishAsync(IEnumerable<IDomainEvent> events)
    {
        foreach (var evt in events)
        {
            await PublishAsync(evt);
        }
    }

    public async Task PublishAsync(IDomainEvent evt)
    {
        var messageType = evt.GetType().FullName ?? throw new InvalidOperationException("Event has no type information.");
        try
        {
            logger.LogInformation("Publishing message of type '{messageType}' ...", messageType);

            var message = SerializeEvent(evt);
            await PublishMessageAsync(messageType, message);

            logger.LogInformation("Successful published message of type '{messageType}'.", messageType);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error publishing message of type '{messageType}'.", messageType);
            throw;
        }
    }

    private async Task PublishMessageAsync(
        string messageType,
        string message
    )
    {
        var attributes = new Dictionary<string, MessageAttributeValue>()
        {
            ["MessageType"] = new MessageAttributeValue
            {
                StringValue = messageType,
                DataType = "String"
            }
        };

        var request = new PublishRequest
        {
            TopicArn = settings.Topic,
            Message = message,
            MessageAttributes = attributes
        };
        var response = await snsClient.PublishAsync(request);
    }

    private string SerializeEvent(IDomainEvent evt)
    {
        return JsonSerializer.Serialize(evt);
    }
}
