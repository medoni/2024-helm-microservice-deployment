using Amazon;
using Amazon.SimpleNotificationService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using POS.Infrastructure.PubSub.Sns.Configurations;
using POS.Shared.Infrastructure.PubSub.Abstractions;

namespace POS.Infrastructure.PubSub.Sns;

/// <summary>
/// Extension methods for adding <see cref="IEventPublisher"/> to <see cref="IServiceCollection"/>
/// </summary>
public static class SnsEventPublisherStartup
{
    /// <summary>
    /// Adds AWS SNS<see cref="IEventPublisher"/> support to the <see cref="IServiceCollection"/>.
    /// </summary>
    public static IServiceCollection AddSnsEventPublisher(
        this IServiceCollection services,
        Action<SnsEventPublisherSettings> options
    )
    {
        services.Configure<SnsEventPublisherSettings>(options);

        services.AddTransient<IEventPublisher, SnsEventPublisher>(svcp =>
        {
            var settings = svcp.GetRequiredService<IOptions<SnsEventPublisherSettings>>().Value;
            var clientConfig = new AmazonSimpleNotificationServiceConfig();
            clientConfig.RegionEndpoint = RegionEndpoint.GetBySystemName(settings.Region);
            var client = new AmazonSimpleNotificationServiceClient(clientConfig);

            return new SnsEventPublisher(
                svcp.GetRequiredService<ILogger<SnsEventPublisher>>(),
                client,
                settings
            );
        });

        return services;
    }
}
