using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using POS.Shared.Domain;
using POS.Shared.Persistence.Repositories;

namespace POS.Domains.Customer.Persistence.DynamoDb.Repositories;
internal abstract class BaseDynamoDbRepository<TAggregate, TAggregateState> : IGenericRepository<TAggregate>
where TAggregate : AggregateRoot
{
    protected IDynamoDBContext DynamoDbCtx { get; }
    protected DynamoDBOperationConfig DynamoDbOperationConfig { get; }

    protected BaseDynamoDbRepository(
        IDynamoDBContext dynamoDBContext,
        DynamoDBOperationConfig dynamoDbOperationConfig
    )
    {
        DynamoDbCtx = dynamoDBContext;
        DynamoDbOperationConfig = dynamoDbOperationConfig;
    }

    public async Task AddAsync(TAggregate aggregate)
    {
        await UpdateAsync(aggregate);
    }

    public async Task<TAggregate> GetByIdAsync(Guid id)
    {
        var batch = DynamoDbCtx.CreateBatchGet<TAggregateState>(DynamoDbOperationConfig);
        batch.AddKey(id);
        await batch.ExecuteAsync();

        var aggregate = CreateAggregate(batch.Results.First());
        return aggregate;
    }

    public async Task UpdateAsync(TAggregate aggregate)
    {
        var aggregateState = aggregate.GetCurrentState<TAggregateState>();

        var batch = DynamoDbCtx.CreateBatchWrite<TAggregateState>(DynamoDbOperationConfig);
        batch.AddPutItem(aggregateState);

        await batch.ExecuteAsync();
    }

    public async IAsyncEnumerable<TAggregate> IterateAsync()
    {
        var query = new QueryOperationConfig
        {

        };
        var search = DynamoDbCtx.FromQueryAsync<TAggregateState>(query, DynamoDbOperationConfig);
        while (!search.IsDone)
        {
            foreach (var item in await search.GetNextSetAsync())
            {
                var aggregate = CreateAggregate(item);
                yield return aggregate;
            }
        }
    }

    protected abstract TAggregate CreateAggregate(TAggregateState state);
}
