namespace POS.Domains.Payment.Service.Domain.Models;
/// <summary>
/// Information about the payer.
/// </summary>
public class Payer
{
    /// <summary>
    /// First name of the payer.
    /// </summary>
    public required string FirstName { get; init; }

    /// <summary>
    /// Last name of the payer.
    /// </summary>
    public required string LastName { get; init; }

    /// <summary>
    /// Email of the payer
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    /// Phone number of the payer
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Address
    /// </summary>
    public PayerAddress? Address { get; set; }
}
