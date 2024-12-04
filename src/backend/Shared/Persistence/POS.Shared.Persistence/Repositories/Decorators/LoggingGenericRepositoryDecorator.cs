using Microsoft.Extensions.Logging;
using POS.Shared.Domain;

namespace POS.Shared.Persistence.Repositories.Decorators;

/// <summary>
/// Responsible for logging calls to to <see cref="IGenericRepository{TAggregate}"/>.
/// </summary>
public abstract class LoggingGenericRepositoryDecorator<TAggregate> : IGenericRepository<TAggregate>
where TAggregate : AggregateRoot
{
    /// <summary>
    /// Next decorator or implementation.
    /// </summary>
    protected IGenericRepository<TAggregate> Next { get; }

    /// <summary>
    /// The logger where to write log messages.
    /// </summary>
    protected ILogger<IGenericRepository<TAggregate>> Logger { get; }

    /// <summary>
    /// Creates a new <see cref="LoggingGenericRepositoryDecorator{TAggregate}"/>.
    /// </summary>
    protected LoggingGenericRepositoryDecorator(
        IGenericRepository<TAggregate> next,
        ILogger<IGenericRepository<TAggregate>> logger
    )
    {
        Next = next ?? throw new ArgumentNullException(nameof(next));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task AddAsync(TAggregate aggregate)
    {
        try
        {
            Logger.LogInformation("Adding aggregate of type '{aggregateType}' with id '{aggregateId}' ...", typeof(TAggregate).Name, aggregate.Id);

            await Next.AddAsync(aggregate);

            Logger.LogInformation("Successfully added aggregate of type '{aggregateType}' with id '{aggregateId}'.", typeof(TAggregate).Name, aggregate.Id);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error adding aggregate of type '{aggregateType}' with id '{aggregateId}'.", typeof(TAggregate).Name, aggregate.Id);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<TAggregate> GetByIdAsync(Guid id)
    {
        try
        {
            Logger.LogInformation("Getting aggregate of type '{aggregateType}' with id '{aggregateId}' ...", typeof(TAggregate).Name, id);

            var aggregate = await Next.GetByIdAsync(id);

            Logger.LogInformation("Successfully got aggregate of type '{aggregateType}' with id '{aggregateId}'.", typeof(TAggregate).Name, id);

            return aggregate;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error getting aggregate of type '{aggregateType}' with id '{aggregateId}'.", typeof(TAggregate).Name, id);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(TAggregate aggregate)
    {
        try
        {
            Logger.LogInformation("Updating aggregate of type '{aggregateType}' with id '{aggregateId}' ...", typeof(TAggregate).Name, aggregate.Id);

            await Next.UpdateAsync(aggregate);

            Logger.LogInformation("Successfully updated aggregate of type '{aggregateType}' with id '{aggregateId}'.", typeof(TAggregate).Name, aggregate.Id);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error updating aggregate of type '{aggregateType}' with id '{aggregateId}'.", typeof(TAggregate).Name, aggregate.Id);
            throw;
        }
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<TAggregate> IterateAsync()
    {
        try
        {
            Logger.LogInformation("Iterating over aggregates of type '{aggregateType}' ...", typeof(TAggregate).Name);

            var result = Next.IterateAsync();

            Logger.LogInformation("Successfully started iteration over aggregates of type '{aggregateType}'.", typeof(TAggregate).Name);

            return result;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error iterating over aggregates of type '{aggregateType}'.", typeof(TAggregate).Name);
            throw;
        }
    }
}
