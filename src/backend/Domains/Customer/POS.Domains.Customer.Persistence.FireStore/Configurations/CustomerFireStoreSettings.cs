namespace POS.Domains.Customer.Persistence.FireStore.Configurations;

/// <summary>
/// Responsible for storing settings about FireStore settings for customers
/// </summary>
public class CustomerFireStoreSettings
{
    /// <summary>
    /// The Google Cloud Project ID where FireStore is located
    /// </summary>
    public required string ProjectId { get; init; }
    
    /// <summary>
    /// Name of FireStore collection for Menus.
    /// </summary>
    public required string MenusCollectionName { get; init; }

    /// <summary>
    /// Name of FireStore collection for Carts.
    /// </summary>
    public required string CartsCollectionName { get; init; }

    /// <summary>
    /// Name of FireStore collection for Orders.
    /// </summary>
    public required string OrdersCollectionName { get; init; }
}