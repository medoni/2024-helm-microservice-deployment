namespace POS.Domains.Payment.Service.Dtos;
/// <summary>
/// Dto with information about the payer.
/// </summary>
public class PayerDto
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
    public PayerAddressDto? Address { get; set; }
}
