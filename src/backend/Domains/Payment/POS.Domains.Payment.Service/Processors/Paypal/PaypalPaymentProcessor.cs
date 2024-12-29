using PaypalServerSdk.Standard;
using PaypalServerSdk.Standard.Models;
using POS.Domains.Customer.Persistence.Orders;
using POS.Domains.Payment.Service.Configurations;
using POS.Domains.Payment.Service.Domain;
using POS.Domains.Payment.Service.Dtos;
using POS.Domains.Payment.Service.Exceptions;
using System.Globalization;

namespace POS.Domains.Payment.Service.Processors.Paypal;

internal class PaypalPaymentProcessor(
    PaypalServerSdkClient paypalClient,
    PaypalProcessorSettings settings,
    IOrderRepository orderRepository
) : IPaymentProcessor
{
    public async Task<PaymentProviderState> RequestPaymentAsync(
        Guid paymentId,
        RequestPaymentDto dto
    )
    {
        var purchaseData = await CreatePurchaseUnitRequest(dto);

        var orderRequest = new OrdersCreateInput()
        {
            Body = new OrderRequest()
            {
                Intent = CheckoutPaymentIntent.Capture,
                PurchaseUnits = new List<PurchaseUnitRequest>()
                {
                    purchaseData.request
                },
                ApplicationContext = new OrderApplicationContext()
                {
                    ReturnUrl = settings.ReturnUrl.Replace("{id}", paymentId.ToString()),
                    CancelUrl = settings.CancelUrl.Replace("{id}", paymentId.ToString())
                }
            }
        };
        var orderResponse = await paypalClient.OrdersController.OrdersCreateAsync(orderRequest);

        var paymentState = new PaypalPaymentProviderState
        {
            Amount = purchaseData.request.Amount.ToGrossNetPriceDto(),
            Description = purchaseData.description,
            Links = orderResponse.Data.Links.ToPosLinks(),
            PaymentProviderId = orderResponse.Data.Id,
            PaypalOrder = orderResponse.Data
        };

        return paymentState;
    }

    internal Task<(PurchaseUnitRequest request, string description)> CreatePurchaseUnitRequest(RequestPaymentDto dto)
    {
        switch (dto.EntityType)
        {
            case EntityTypes.CustomerOrder: return CreatePurchaseUnitRequestFromOrder(dto);
            default: throw new NotImplementedException($"The entity type '{dto.EntityType}' is not suitable for Paypal payments.");
        }
    }

    internal async Task<(PurchaseUnitRequest request, string description)> CreatePurchaseUnitRequestFromOrder(RequestPaymentDto dto)
    {
        var order = await orderRepository.GetByIdAsync(Guid.Parse(dto.EntityId));
        var paymentDescription = "Lorem Ipsum Description"; // TODO:

        var purchaseRequest = new PurchaseUnitRequest()
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

        return (
            purchaseRequest,
            paymentDescription
        );
    }

    public async Task<PaymentProviderState> CapturePaymentAsync(PaymentEntity paymentEntity)
    {
        var providerState = (PaypalPaymentProviderState)paymentEntity.ProviderState;

        var captureInput = new OrdersCaptureInput
        {
            Id = providerState.PaymentProviderId
        };

        var captureResult = await paypalClient.OrdersController.OrdersCaptureAsync(
            captureInput
        );

        if (captureResult.Data.Status != OrderStatus.Completed)
        {
            throw new PaymentCaptureNotCompletedException(paymentEntity.Id, captureResult.Data.Status?.ToString() ?? "<NULL>");
        }

        providerState.PayedAt = DateTimeOffset.Parse(
            captureResult.Data.PurchaseUnits.Last().Payments.Captures.Last().CreateTime, CultureInfo.InvariantCulture
        );
        providerState.CapturedAt = DateTimeOffset.UtcNow;

        return providerState;
    }
}
