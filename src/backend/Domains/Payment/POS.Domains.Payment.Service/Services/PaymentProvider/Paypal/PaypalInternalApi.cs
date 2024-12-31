using PaypalServerSdk.Standard;
using PaypalServerSdk.Standard.Models;

namespace POS.Domains.Payment.Service.Services.PaymentProvider.Paypal;
internal class PaypalInternalApi(
    PaypalServerSdkClient paypalClient
) : IPaypalInternalApi
{
    public async Task<Order> OrdersCreateAsync(OrderRequest request)
    {
        var response = await paypalClient.OrdersController.OrdersCreateAsync(new() { Body = request });
        return response.Data;
    }
}
