using POS.Shared.Domain;

namespace POS.Shared.Persistence.UOW;
public interface IUnitOfWork
{
    Task<TAggregate> GetAsync<TAggregate>(Guid id)
        where TAggregate : AggregateRoot<Guid>;

    void Add<TAggregate>(TAggregate aggregate)
        where TAggregate : AggregateRoot<Guid>;

    Task CommitAsync();
}
