using Microsoft.Extensions.Options;
using POS.Domains.Customer.Domain.Orders;
using POS.Domains.Payment.Service.Domain;
using POS.Domains.Payment.Service.Domain.Models;
using POS.Shared.Domain.Generic.Mapper;
using POS.Shared.Persistence.UOW;
using System.Text.Json;

namespace POS.Domains.Payment.Service.Services.PaymentProvider.Paypal;

/// <summary>
/// Responsible for processing payments for Order-aggregates
/// </summary>
internal class PaypalPaymentOrderProvider(
    UnitOfWorkFactory uowFactory,
    IPaypalInternalApi paypalApi,
    IOptions<PaypalPaymentSettings> paypalSettings
) : IPaymentProvider
{
    public async Task<PaymentAggregate> RequestPaymentAsync(
        string entityId,
        DateTimeOffset requestAt
    )
    {
        var uow = uowFactory();
        PaymentAggregate payment;

        var order = await uow.GetAsync<Order>(Guid.Parse(entityId));
        order.CheckPaymentCanBeRequested();

        if (order.PaymentInfo is null)
        {
            payment = PaymentAggregate.Create(
                PaymentProviderTypes.Paypal,
                EntityTypes.CustomerOrder,
                entityId,
                requestAt,
                "Lorem ipsum order",
                order.PriceSummary.TotalPrice.ToDomain()
            );
            uow.Add(payment);
        }
        else
        {
            payment = await uow.GetAsync<PaymentAggregate>(order.PaymentInfo.PaymentId);
        }

        payment.CheckPaymentCanBeRequested();
        var paypalState = await RequestPaymentPaypalApiAsync(payment, order);
        var approvalLink = paypalState.Links.ToPosLinks().FirstOrDefault(x => x.Type == PaymentLinkTypes.Approve) ?? throw new InvalidOperationException("No approval link found.");
        payment.PaymentRequested(
            paypalState.TotalAmountRequested.ToDomain(),
            requestAt,
            approvalLink,
            SerializeInternalState(paypalState)
        );

        order.PaymentRequested(
            payment.Id,
            requestAt
        );

        await uow.CommitAsync();

        return payment;
    }

    private async Task<PaypalPaymentInternalState> RequestPaymentPaypalApiAsync(
        PaymentAggregate payment,
        Order order
    )
    {
        var purchaseData = CreatePurchaseUnitRequestFromOrder(order);

        var orderRequest = new PaypalServerSdk.Standard.Models.OrderRequest()
        {
            Intent = PaypalServerSdk.Standard.Models.CheckoutPaymentIntent.Capture,
            PurchaseUnits = new()
                {
                    purchaseData
                },
            ApplicationContext = new()
            {
                ReturnUrl = paypalSettings.Value.ReturnUrl.Replace("{id}", payment.Id.ToString()),
                CancelUrl = paypalSettings.Value.CancelUrl.Replace("{id}", payment.Id.ToString())
            }
        };
        var orderResponse = await paypalApi.OrdersCreateAsync(orderRequest);
        var paymentState = new PaypalPaymentInternalState
        {
            PaypalId = orderResponse.Id,
            CreatedAt = DateTimeOffset.UtcNow,
            LastChangedAt = DateTimeOffset.UtcNow,
            OrderStatus = orderResponse.Status!.Value,
            TotalAmountRequested = purchaseData.Amount.ToGrossNetPriceDto(),
            Links = orderResponse.Links
        };

        return paymentState;
    }

    internal static PaypalServerSdk.Standard.Models.PurchaseUnitRequest CreatePurchaseUnitRequestFromOrder(Order order)
    {
        var paymentDescription = "Lorem Ipsum Description"; // TODO:

        var purchaseRequest = new PaypalServerSdk.Standard.Models.PurchaseUnitRequest()
        {
            CustomId = order.Id.ToString(),
            InvoiceId = Guid.NewGuid().ToString(),
            Description = paymentDescription,
            //Payee = settings.Payee,
            Items = order.OrderItems.ToPaypalItems(),
        };
        purchaseRequest.Amount = purchaseRequest.Items.CalculateAmountWithBreakdown(
            order.PriceSummary.Discount,
            order.PriceSummary.DeliveryCosts
        );

        return purchaseRequest;
    }

    private static string SerializeInternalState(PaypalPaymentInternalState state)
    {
        return JsonSerializer.Serialize(state);
    }
}
