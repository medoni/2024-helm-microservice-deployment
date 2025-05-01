using Google.Cloud.Firestore;
using POS.Domains.Customer.Domain.Menus;
using POS.Domains.Customer.Persistence.FireStore.Entities;
using POS.Domains.Customer.Persistence.Menus;

namespace POS.Domains.Customer.Persistence.FireStore.Repositories;

internal class MenuRepository : BaseFireStoreRepository<Menu, MenuEntity>, IMenuRespository
{
    public MenuRepository(
        FirestoreDb firestoreDb,
        string collectionName
    ) : base(firestoreDb, collectionName)
    {
    }

    protected override MenuEntity CreateFireStoreEntity(Menu aggregate)
    {
        var entity = new MenuEntity
        {
            Id = aggregate.Id.ToString(),
            Name = aggregate.Name,
            Description = aggregate.Description,
            Items = aggregate.Items.Select(i => new MenuItemEntity
            {
                Id = i.Id.ToString(),
                Name = i.Name,
                Description = i.Description,
                Price = i.Price
            }).ToList()
        };

        return entity;
    }

    protected override Menu CreateAggregate(MenuEntity entity)
    {
        var menuItems = entity.Items.Select(i => new MenuItem(
            Guid.Parse(i.Id),
            i.Name,
            i.Description,
            i.Price
        )).ToList();

        var menu = new Menu(
            Guid.Parse(entity.Id),
            entity.Name,
            entity.Description,
            menuItems
        );

        return menu;
    }
}