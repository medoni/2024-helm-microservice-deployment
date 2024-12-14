namespace POS.Domains.Customer.Persistence.DynamoDb.Configurations;
/// <summary>
/// Responsible for storing settings about DynamoDb settings for customers
/// </summary>
public class CustomerDynamoDbSettings
{
    /// <summary>
    /// Region in which are the DynamoDb tables are stored.
    /// </summary>
    public required string Region { get; init; }

    /// <summary>
    /// Name of DEynamoDb table for Menus.
    /// </summary>
    public required string MenusTableName { get; init; }

    /// <summary>
    /// Name of DEynamoDb table for Carts.
    /// </summary>
    public required string CartsTableName { get; init; }

    /// <summary>
    /// Name of DEynamoDb table for Orders.
    /// </summary>
    public required string OrdersTableName { get; init; }
}
