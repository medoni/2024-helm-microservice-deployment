using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using Testcontainers.DynamoDb;

namespace POS.Domains.Customer.Persistence.DynamoDb.Tests.Repositories;

[TestFixture]
public abstract class BaseDynamoDbRepositoryIntegrationTests
{
    protected DynamoDbContainer DynamoDbContainer { get; set; }
    protected AmazonDynamoDBClient DynamoDbClient { get; set; }
    protected DynamoDBContext DynamoDbContext { get; set; }
    protected DynamoDBOperationConfig DynamoDbOperationConfig { get; set; }

    [OneTimeSetUp]
    public virtual async Task OneTimeSetupAsync()
    {
        DynamoDbContainer = new DynamoDbBuilder()
          .WithImage("amazon/dynamodb-local:2.5.3")
          .Build();
        await DynamoDbContainer.StartAsync();

        DynamoDbClient = new AmazonDynamoDBClient(
            new BasicAWSCredentials("test", "test"),
            new AmazonDynamoDBConfig
            {
                ServiceURL = DynamoDbContainer.GetConnectionString(),
            });
        DynamoDbContext = new DynamoDBContext(DynamoDbClient);

        DynamoDbOperationConfig = new DynamoDBOperationConfig()
        {
            OverrideTableName = "test-table"
        };
        await CreateTableAsync(DynamoDbOperationConfig);
    }

    protected abstract Task CreateTableAsync(DynamoDBOperationConfig operationConfig);

    [OneTimeTearDown]
    public virtual async Task OneTimeTearDownAsync()
    {
        DynamoDbContext.Dispose();
        DynamoDbClient.Dispose();
        await DynamoDbContainer.DisposeAsync();
    }
}
