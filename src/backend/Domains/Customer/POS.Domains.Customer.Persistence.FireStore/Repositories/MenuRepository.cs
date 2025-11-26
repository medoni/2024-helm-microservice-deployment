using Google.Cloud.Firestore;
using POS.Domains.Customer.Domain.Menus;
using POS.Domains.Customer.Persistence.FireStore.Entities;
using POS.Domains.Customer.Persistence.Menus;
using System.Text.Json;

namespace POS.Domains.Customer.Persistence.FireStore.Repositories;

internal class MenuRepository : BaseFireStoreRepository<Menu, MenuEntity>, IMenuRespository
{
    public MenuRepository(
        FirestoreDb firestoreDb,
        string collectionName
    ) : base(firestoreDb, collectionName)
    {
    }

    public async Task<Menu?> GetActiveAsync()
    {
        // TODO: slow
        await foreach (var menu in IterateAsync())
        {
            if (menu.IsActive) return menu;
        }

        return null;
    }

    protected override Menu CreateAggregate(MenuEntity entity)
    {
        var state = JsonSerializer.Deserialize<MenuState>(entity.Payload)!;
        var aggregate = new Menu(state);
        return aggregate;
    }

    protected override MenuEntity CreateFireStoreEntity(Menu aggregate)
    {
        var state = aggregate.GetCurrentState<MenuState>();

        var entity = new MenuEntity
        {
            Id = aggregate.Id.ToString(),
            CreatedAt = aggregate.CreatedAt.ToString("O"),
            Payload = JsonSerializer.Serialize(state),
            Active = state.IsActive ? 1 : 0
        };
        return entity;
    }
}
