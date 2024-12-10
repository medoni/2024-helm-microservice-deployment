namespace POS.Domains.Customer.Persistence.DynamoDb.Configurations;
/// <summary>
///
/// </summary>
public class CustomerDynamoDbOptions
{
    /// <summary>
    ///
    /// </summary>
    public required string Region { get; init; }

    /// <summary>
    ///
    /// </summary>
    public required string MenusTableName { get; init; }

    /// <summary>
    ///
    /// </summary>
    public required string CartsTableName { get; init; }

    /// <summary>
    ///
    /// </summary>
    public required string OrdersTableName { get; init; }
}
