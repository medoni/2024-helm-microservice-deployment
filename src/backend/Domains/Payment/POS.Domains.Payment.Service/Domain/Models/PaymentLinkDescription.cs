using System.Diagnostics.CodeAnalysis;

namespace POS.Domains.Payment.Service.Domain.Models;

/// <summary>
/// Link description for a payment.
/// </summary>
public class PaymentLinkDescription
{
    /// <summary>
    /// Url
    /// </summary>
    public required string Url { get; init; }

    /// <summary>
    /// Type of the url.
    /// </summary>
    public required PaymentLinkTypes Type { get; init; }

    /// <summary>
    /// Method of the url.
    /// </summary>
    public required PaymentLinkMethods Method { get; init; }

    /// <summary>
    /// Creates a new <see cref="PaymentLinkDescription"/>.
    /// </summary>
    public PaymentLinkDescription()
    {
    }

    /// <summary>
    /// Creates a new <see cref="PaymentLinkDescription"/>.
    /// </summary>
    [SetsRequiredMembers]
    public PaymentLinkDescription(
        string url,
        PaymentLinkTypes type,
        PaymentLinkMethods method
    )
    {
        Url = url ?? throw new ArgumentNullException(nameof(url));
        Type = type;
        Method = method;
    }
}
