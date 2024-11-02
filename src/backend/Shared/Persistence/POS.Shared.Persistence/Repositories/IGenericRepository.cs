using POS.Shared.Domain;

namespace POS.Shared.Persistence.Repositories;

public interface IGenericRepository<TAggregate, TID>
where TAggregate : AggregateRoot<TID>
{
    Task AddAsync(TAggregate aggregate);
    Task<TAggregate> GetByIdAsync(TID id);
    Task UpdateAsync(TAggregate aggregate);
    IAsyncEnumerable<TAggregate> IterateAsync();
}
