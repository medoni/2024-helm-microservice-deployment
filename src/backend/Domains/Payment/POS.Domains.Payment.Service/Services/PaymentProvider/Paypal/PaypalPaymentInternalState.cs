using PaypalServerSdk.Standard.Models;
using POS.Shared.Domain.Generic.Dtos;

namespace POS.Domains.Payment.Service.Services.PaymentProvider.Paypal;
internal class PaypalPaymentInternalState
{
    public required string PaypalId { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required DateTimeOffset LastChangedAt { get; set; }
    public required OrderStatus OrderStatus { get; set; }

    public required GrossNetPriceDto TotalAmountRequested { get; init; }
    public required List<LinkDescription> Links { get; set; } = new();
}
