namespace POS.Domains.Payment.Service.Domain.Models;

/// <summary>
/// Address of the payer
/// </summary>
public class PayerAddress
{
    /// <summary>
    /// Address line 1
    /// </summary>
    public required string AddressLine1 { get; init; }

    /// <summary>
    /// Address line 2
    /// </summary>
    public string? AddressLine2 { get; init; }

    /// <summary>
    /// City
    /// </summary>
    public required string City { get; init; }

    /// <summary>
    /// Zip code
    /// </summary>
    public required string Zip { get; init; }

    /// <summary>
    /// Country code. For example `DE` for Germany
    /// </summary>
    public required string CountryCode { get; init; }
}
