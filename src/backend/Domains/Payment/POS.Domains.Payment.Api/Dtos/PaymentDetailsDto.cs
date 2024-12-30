using POS.Domains.Payment.Service.Domain;
using POS.Domains.Payment.Service.Services.PaymentProvider;
using POS.Shared.Domain.Generic.Dtos;
using POS.Shared.Domain.Generic.Mapper;

namespace POS.Domains.Payment.Api.Dtos;

/// <summary>
/// Dto containing details for a payment
/// </summary>
public class PaymentDetailsDto
{
    /// <summary>
    /// Id of the payment.
    /// </summary>
    public required Guid PaymentId { get; init; }

    /// <summary>
    /// ID of the entity for which the payment should be requested.
    /// </summary>
    public required string EntityId { get; init; }

    /// <summary>
    /// Type of the entity for which the payment should be requested
    /// </summary>
    public required EntityTypes EntityType { get; init; }

    /// <summary>
    /// Date and time when the payment was created.
    /// </summary>
    public required DateTimeOffset CreatedAt { get; init; }

    /// <summary>
    /// Date and time when the request was started.
    /// </summary>
    public DateTimeOffset? RequestedAt { get; init; }

    /// <summary>
    /// State of the payment
    /// </summary>
    public required PaymentStates State { get; init; }

    /// <summary>
    /// Payment provider.
    /// </summary>
    public required PaymentProviderTypes Provider { get; init; }

    /// <summary>
    /// Description of the payment
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// Amount of the payment.
    /// </summary>
    public required GrossNetPriceDto Amount { get; init; }

    /// <summary>
    /// Allowed links for the payment
    /// </summary>
    public required List<PaymentLinkDescription> Links { get; init; }

    /// <summary>
    /// Date and time when the payment was successfully approved.
    /// </summary>
    public DateTimeOffset? ApprovedAt { get; init; }

    /// <summary>
    /// Date and time when the payment was successfully captured.
    /// </summary>
    public DateTimeOffset? CapturedAt { get; init; }
}

internal static class PaymentDetailsDtoExtensions
{
    public static PaymentDetailsDto ToPaymentDetailsDto(
        this PaymentAggregate aggregate
    )
    {
        return new PaymentDetailsDto
        {
            PaymentId = aggregate.Id,
            Provider = aggregate.PaymentProvider,
            State = aggregate.State,
            EntityType = aggregate.EntityType,
            EntityId = aggregate.EntityId,
            Description = aggregate.Description,
            Amount = aggregate.TotalAmount.ToDto(),
            CreatedAt = aggregate.CreatedAt,
            // TODO: RequestedAt = aggregate.RequestedAt,
            // TODO: CapturedAt = aggregate.CapturedAt,
            // TODO: ApprovedAt = aggregate.ApprovedAt,
            Links = aggregate.Links.ToList(),
        };
    }
}
