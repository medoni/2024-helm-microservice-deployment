using PaypalServerSdk.Standard.Models;

namespace POS.Domains.Payment.Api.Dtos;
/// <summary>
///
/// </summary>
public class PaypalOrderDto
{
    /// <summary>
    ///
    /// </summary>
    public required Guid PosOrderId { get; init; }

    /// <summary>
    ///
    /// </summary>
    public required string PaypalOrderId { get; init; }

    /// <summary>
    ///
    /// </summary>
    public required List<LinkDescription> PaypalLinks { get; init; }
}
