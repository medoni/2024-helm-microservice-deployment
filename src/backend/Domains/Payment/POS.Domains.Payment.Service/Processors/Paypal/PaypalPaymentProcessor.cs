using PaypalServerSdk.Standard;
using PaypalServerSdk.Standard.Models;
using POS.Domains.Customer.Persistence.Orders;
using POS.Domains.Payment.Service.Configurations;
using POS.Domains.Payment.Service.Domain;
using POS.Domains.Payment.Service.Dtos;
using POS.Shared.Domain.Generic.Dtos;

namespace POS.Domains.Payment.Service.Processors.Paypal;

internal class PaypalPaymentProcessor(
    PaypalServerSdkClient paypalClient,
    PaypalProcessorSettings settings,
    IOrderRepository orderRepository
) : IPaymentProcessor
{

    public async Task<PaymentProviderState> RequestPaymentAsync(RequestPaymentDto dto)
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
                    ReturnUrl = settings.ReturnUrl,
                    CancelUrl = settings.CancelUrl
                }
            }
        };
        var orderResponse = await paypalClient.OrdersController.OrdersCreateAsync(orderRequest);

        var paymentState = new PaypalPaymentProviderState
        {
            Amount = purchaseData.totalPrice,
            Description = purchaseData.description,
            Links = orderResponse.Data.Links.ToPosLinks(),
            PaymentProviderId = orderResponse.Data.Id,
            PaypalOrder = orderResponse.Data
        };

        return paymentState;
    }

    internal Task<(PurchaseUnitRequest request, GrossNetPriceDto totalPrice, string description)> CreatePurchaseUnitRequest(RequestPaymentDto dto)
    {
        switch (dto.EntityType)
        {
            case EntityTypes.CustomerOrder: return CreatePurchaseUnitRequestFromOrder(dto);
            default: throw new NotImplementedException($"The entity type '{dto.EntityType}' is not suitable for Paypal payments.");
        }
    }

    internal async Task<(PurchaseUnitRequest request, GrossNetPriceDto totalPrice, string description)> CreatePurchaseUnitRequestFromOrder(RequestPaymentDto dto)
    {
        var order = await orderRepository.GetByIdAsync(Guid.Parse(dto.EntityId));
        var paymentDescription = "Lorem Ipsum Description"; // TODO:

        return (
            new PurchaseUnitRequest()
            {
                CustomId = order.Id.ToString(),
                InvoiceId = order.Id.ToString(),
                Description = paymentDescription,
                //Payee = settings.Payee,
                Items = order.OrderItems.ToPaypalItems(),
                Amount = new()
                {
                    CurrencyCode = order.PriceSummary.TotalPrice.ToPaypalMoney().CurrencyCode,
                    MValue = order.PriceSummary.TotalPrice.ToPaypalMoney().MValue,
                    Breakdown = new AmountBreakdown
                    {
                        ItemTotal = order.PriceSummary.TotalItemPrice.ToPaypalMoney(),
                        // TODO: https://github.com/medoni/2024-helm-microservice-deployment/issues/18
                        TaxTotal = order.OrderItems.CalculateTaxTotal(),
                        Discount = order.PriceSummary.Discount.ToPaypalMoney(),
                        Shipping = order.PriceSummary.DeliveryCosts.ToPaypalMoney(),
                    }
                }
            },
            order.PriceSummary.TotalPrice,
            paymentDescription
        );
    }
}
