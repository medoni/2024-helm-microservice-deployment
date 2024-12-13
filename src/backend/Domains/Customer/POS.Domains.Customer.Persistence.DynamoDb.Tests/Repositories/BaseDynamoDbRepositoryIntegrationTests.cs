using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
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
          .WithImage("amazon/dynamodb-local:1.21.0")
          .Build();
        await DynamoDbContainer.StartAsync();

        var logs = await DynamoDbContainer.GetLogsAsync();
        Console.WriteLine("Container.ServiceUrl: " + DynamoDbContainer.GetConnectionString());
        Console.WriteLine("Container logs stdout: " + logs.Stdout);
        Console.WriteLine("Container logs stderr: " + logs.Stderr);

        DynamoDbClient = new AmazonDynamoDBClient(new AmazonDynamoDBConfig
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
