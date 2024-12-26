namespace POS.Domains.Payment.Api.Dtos;
/// <summary>
///
/// </summary>
public class BeginPaymentDto
{
    /// <summary>
    ///
    /// </summary>
    public required Guid OrderId { get; init; }
}
