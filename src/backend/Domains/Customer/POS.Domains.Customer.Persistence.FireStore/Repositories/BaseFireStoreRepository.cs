using Google.Cloud.Firestore;
using POS.Shared.Domain;
using POS.Shared.Persistence.Repositories;

namespace POS.Domains.Customer.Persistence.FireStore.Repositories;

internal abstract class BaseFireStoreRepository<TAggregate, TEntity> : IGenericRepository<TAggregate>
where TAggregate : AggregateRoot
where TEntity : class
{
    protected FirestoreDb FireStoreDb { get; }
    protected string CollectionName { get; }

    protected BaseFireStoreRepository(
        FirestoreDb firestoreDb,
        string collectionName
    )
    {
        FireStoreDb = firestoreDb;
        CollectionName = collectionName;
    }

    public async Task AddAsync(TAggregate aggregate)
    {
        await UpdateAsync(aggregate);
    }

    public async Task<TAggregate> GetByIdAsync(Guid id)
    {
        var docRef = FireStoreDb.Collection(CollectionName).Document(id.ToString());
        var snapshot = await docRef.GetSnapshotAsync();

        if (!snapshot.Exists)
        {
            throw new KeyNotFoundException($"Entity with id {id} not found");
        }

        var entity = snapshot.ConvertTo<TEntity>();
        var aggregate = CreateAggregate(entity);
        return aggregate;
    }

    public async Task UpdateAsync(TAggregate aggregate)
    {
        var entity = CreateFireStoreEntity(aggregate);
        var docRef = FireStoreDb.Collection(CollectionName).Document(aggregate.Id.ToString());

        await docRef.SetAsync(entity);
    }

    protected abstract TEntity CreateFireStoreEntity(TAggregate aggregate);

    public async IAsyncEnumerable<TAggregate> IterateAsync()
    {
        var collection = FireStoreDb.Collection(CollectionName);
        var querySnapshot = await collection.GetSnapshotAsync();

        foreach (var document in querySnapshot.Documents)
        {
            var entity = document.ConvertTo<TEntity>();
            var aggregate = CreateAggregate(entity);
            yield return aggregate;
        }
    }

    protected abstract TAggregate CreateAggregate(TEntity state);
}
