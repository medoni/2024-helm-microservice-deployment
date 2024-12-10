using POS.Shared.Persistence.Repositories;
using POS.Shared.Persistence.UOW;

namespace POS.Domains.Customer.Persistence.DynamoDb.UnitOfWork;
internal class DynamoDbUnitOfWork : BaseUnitOfWork
{
    public DynamoDbUnitOfWork(
        IServiceProvider serviceProvider,
        BaseRepositoryFactory repositoryFactory
    ) : base(serviceProvider, repositoryFactory)
    {
    }

    protected override TrackedRecord CreateAddRecord<TAggregate>(TAggregate aggregate, Func<IGenericRepository<TAggregate>> getRepo)
    {
        var commitAction = async () =>
        {
            var repo = getRepo();
            await repo.AddAsync(aggregate);
        };

        return new TrackedRecord(
            aggregate,
            commitAction
        );
    }

    protected override TrackedRecord CreateUpdateRecord<TAggregate>(TAggregate aggregate, Func<IGenericRepository<TAggregate>> getRepo)
    {
        var commitAction = async () =>
        {
            var repo = getRepo();
            await repo.UpdateAsync(aggregate);
        };

        return new TrackedRecord(
            aggregate,
            commitAction
        );
    }

    protected override Task FlushCommitsAsync()
    {
        return Task.CompletedTask;
    }
}
