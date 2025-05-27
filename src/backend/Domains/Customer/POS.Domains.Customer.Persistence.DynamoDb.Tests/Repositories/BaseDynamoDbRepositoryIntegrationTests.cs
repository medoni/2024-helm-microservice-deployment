using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using POS.Domains.Customer.Persistence.DynamoDb.Repositories;
using POS.Shared.Domain;
using POS.Shared.Domain.Events;
using POS.Shared.Testing;

namespace POS.Domains.Customer.Persistence.DynamoDb.Tests.Repositories;
[TestFixture]
[Category(TestCategories.Unit)]
public class BaseDynamoDbRepositoryIntegrationTests : BaseDynamoDbRepositoryFixture
{
    private SutRepo Sut { get; set; }

    [SetUp]
    public void SetUp()
    {
        Sut = new SutRepo(
            DynamoDbContext,
            DynamoDbOperationConfig
        );
    }

    [Test]
    public async Task Update_Should_Store_Data_In_Batches()
    {
        // arrange
        var entity = new SutAggregate(Guid.NewGuid(), "Foo");

        // act
        await Sut.UpdateAsync(entity);

        // assert
        var storedEntity = await Sut.GetByIdAsync(entity.Id);
        Assert.That(storedEntity, Is.Not.Null);
    }

    [Test]
    public async Task Update_Without_Changes_Should_Not_Store_Any_Data()
    {
        // arrange
        var entity = new SutAggregate(Guid.NewGuid(), "Foo");
        entity.FlushUncommittedChanges();

        // act
        await Sut.UpdateAsync(entity);

        // assert
        Assert.That(
            () => Sut.GetByIdAsync(entity.Id),
            Throws.InvalidOperationException
        );
    }

    #region Aggregate & Repository definition

    private class SutAggregate : AggregateRoot
    {
        private SutEntity _internalState;
        public override TState GetCurrentState<TState>() => (TState)(object)_internalState;

        public override Guid Id => Guid.Parse(_internalState.Id);
        public string Name
        {
            get => _internalState.Name;
            private set => _internalState.Name = value;
        }

        public int ValueInt
        {
            get => _internalState.ValueInt;
            private set => _internalState.ValueInt = value;
        }

        public SutAggregate(
            SutEntity entity
        )
        {
            _internalState = entity;
        }
        public SutAggregate(
            Guid id,
            string name
        )
        {
            _internalState = new SutEntity() { Id = id.ToString() };
            Apply(new PropertyChangedEvent(Id, nameof(Id), id));

            ChangeName(name);
        }

        public void ChangeName(string newValue)
        {
            Name = newValue;
            Apply(new PropertyChangedEvent(Id, nameof(Name), newValue));
        }

        public void ChangeValueInt(int newValue)
        {
            ValueInt = newValue;
            Apply(new PropertyChangedEvent(Id, nameof(ValueInt), newValue));
        }
    }

    private record PropertyChangedEvent(
        Guid Id,
        string PropertyName,
        object NewValue
    ) : IDomainEvent
    { }

    private class SutEntity
    {
        [DynamoDBHashKey]
        [DynamoDBProperty("id")]
        public string Id { get; set; }

        [DynamoDBProperty("name")]
        public string Name { get; set; }

        [DynamoDBProperty("ValueInt")]
        public int ValueInt { get; set; }
    }

    private class SutRepo : BaseDynamoDbRepository<SutAggregate, SutEntity>
    {
        public SutRepo(
            IDynamoDBContext dynamoDBContext,
            DynamoDBOperationConfig dynamoDbOperationConfig
        ) : base(dynamoDBContext, dynamoDbOperationConfig)
        {
        }

        protected override SutAggregate CreateAggregate(SutEntity state)
        {
            return new SutAggregate(state);
        }

        protected override SutEntity CreateDynamoDbEntity(SutAggregate aggregate)
        {
            return aggregate.GetCurrentState<SutEntity>();
        }
    }

    #endregion

    #region Helpers

    protected override async Task CreateTableAsync(DynamoDBOperationConfig operationConfig)
    {
        try
        {
            var createTableRequest = new CreateTableRequest
            {
                TableName = operationConfig.OverrideTableName,
                BillingMode = BillingMode.PAY_PER_REQUEST,
                KeySchema = new()
            {
                new KeySchemaElement("id", KeyType.HASH)
            },
                AttributeDefinitions = new()
            {
                new AttributeDefinition() { AttributeName = "id", AttributeType = "S" }
            }
            };
            await DynamoDbClient.CreateTableAsync(createTableRequest);
        }
        catch (Exception ex)
        {
            var logs = await DynamoDbContainer.GetLogsAsync();

            throw new InvalidOperationException(
                $"""
                DynamoDbClient.GetConnectionString(): '{DynamoDbContainer.GetConnectionString()}'
                -------------------------------------------
                Stdout:
                {logs.Stdout}
                -------------------------------------------
                Stderr:
                {logs.Stderr}
                -------------------------------------------
                """, ex
            );
        }
    }

    #endregion
}
