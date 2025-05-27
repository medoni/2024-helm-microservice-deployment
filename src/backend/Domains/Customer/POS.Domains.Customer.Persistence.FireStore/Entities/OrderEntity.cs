using Google.Cloud.Firestore;

namespace POS.Domains.Customer.Persistence.FireStore.Entities;

[FirestoreData]
internal class OrderEntity
{
    [FirestoreDocumentId]
    public required string Id { get; set; }

    [FirestoreProperty]
    public required string CreatedAt { get; set; }

    [FirestoreProperty]
    public required string Payload { get; set; }
}
