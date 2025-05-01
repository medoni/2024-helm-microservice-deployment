using Google.Cloud.Firestore;

namespace POS.Domains.Customer.Persistence.FireStore.Entities;

[FirestoreData]
internal class CartEntity
{
    [FirestoreDocumentId]
    public string Id { get; set; }
    
    [FirestoreProperty]
    public string CustomerId { get; set; }
    
    [FirestoreProperty]
    public List<CartItemEntity> Items { get; set; } = new();
    
    [FirestoreProperty]
    public decimal TotalPrice { get; set; }
}

[FirestoreData]
internal class CartItemEntity
{
    [FirestoreProperty]
    public string MenuItemId { get; set; }
    
    [FirestoreProperty]
    public string Name { get; set; }
    
    [FirestoreProperty]
    public int Quantity { get; set; }
    
    [FirestoreProperty]
    public decimal UnitPrice { get; set; }
}