namespace POS.Infrastructure.PubSub.Sns.Configurations;

/// <summary>
/// Settings for configuring <see cref="SnsEventPublisher"/>
/// </summary>
public class SnsEventPublisherSettings
{
    /// <summary>
    /// AWS Region
    /// </summary>
    public required string Region { get; set; }

    /// <summary>
    /// Name of the topic to publish to.
    /// </summary>
    public required string Topic { get; set; }
}
