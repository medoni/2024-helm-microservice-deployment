using Amazon.DynamoDBv2.DataModel;

namespace POS.Domains.Customer.Persistence.DynamoDb.Entities;
internal class CartEntity
{
    [DynamoDBHashKey]
    [DynamoDBProperty("id")]
    public required string Id { get; set; }

    [DynamoDBProperty("createdAt")]
    public required string CreatedAt { get; set; }

    [DynamoDBProperty("payload")]
    public required string Payload { get; set; }
}
