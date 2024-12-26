namespace POS.Domains.Payment.Api.Dtos;
/// <summary>
///
/// </summary>
public class CaptureOrderDto
{
    /// <summary>
    ///
    /// </summary>
    public required Guid PosOrderId { get; init; }

    /// <summary>
    ///
    /// </summary>
    public required string PaypalOrderId { get; init; }
}
