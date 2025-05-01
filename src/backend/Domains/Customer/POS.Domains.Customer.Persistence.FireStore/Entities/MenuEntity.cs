using Google.Cloud.Firestore;

namespace POS.Domains.Customer.Persistence.FireStore.Entities;

[FirestoreData]
internal class MenuEntity
{
    [FirestoreDocumentId]
    public string Id { get; set; }
    
    [FirestoreProperty]
    public string Name { get; set; }
    
    [FirestoreProperty]
    public string Description { get; set; }
    
    [FirestoreProperty]
    public List<MenuItemEntity> Items { get; set; } = new();
}

[FirestoreData]
internal class MenuItemEntity
{
    [FirestoreProperty]
    public string Id { get; set; }
    
    [FirestoreProperty]
    public string Name { get; set; }
    
    [FirestoreProperty]
    public string Description { get; set; }
    
    [FirestoreProperty]
    public decimal Price { get; set; }
}