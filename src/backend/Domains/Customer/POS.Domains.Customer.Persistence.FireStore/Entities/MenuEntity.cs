using Google.Cloud.Firestore;

namespace POS.Domains.Customer.Persistence.FireStore.Entities;

[FirestoreData]
internal class MenuEntity
{
    [FirestoreDocumentId]
    public required string Id { get; set; }

    [FirestoreProperty]
    public required string CreatedAt { get; set; }

    [FirestoreProperty]
    public required string Payload { get; set; }

    [FirestoreProperty]
    public required int Active { get; set; }
}
