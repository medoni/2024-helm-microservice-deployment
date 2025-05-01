using Google.Cloud.Firestore;

namespace POS.Domains.Customer.Persistence.FireStore.Entities;

[FirestoreData]
internal class OrderEntity
{
    [FirestoreDocumentId]
    public string Id { get; set; }
    
    [FirestoreProperty]
    public string CustomerId { get; set; }
    
    [FirestoreProperty]
    public List<OrderItemEntity> Items { get; set; } = new();
    
    [FirestoreProperty]
    public decimal TotalPrice { get; set; }
    
    [FirestoreProperty]
    public string Status { get; set; }
    
    [FirestoreProperty]
    public Timestamp OrderDate { get; set; }
}

[FirestoreData]
internal class OrderItemEntity
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