using Microsoft.EntityFrameworkCore;
using POS.Shared.Persistence.Repositories;
using POS.Shared.Persistence.UOW;

namespace POS.Shared.Persistence.PostgreSql.UnitOfWork;
internal class EfCoreUnitOfWork<TDbContext> : BaseUnitOfWork
where TDbContext : DbContext
{
    private readonly DbContext _dbContext;

    public EfCoreUnitOfWork(
        IServiceProvider serviceProvider,
        TDbContext dbContext,
        BaseRepositoryFactory repositoryFactory
    ) : base(serviceProvider, repositoryFactory)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
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

    protected override async Task FlushCommitsAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
