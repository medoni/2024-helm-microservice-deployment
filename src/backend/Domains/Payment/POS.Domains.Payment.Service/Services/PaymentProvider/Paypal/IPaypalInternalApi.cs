using PaypalServerSdk.Standard.Models;

namespace POS.Domains.Payment.Service.Services.PaymentProvider.Paypal;

/// <summary>
/// Internal api for paypal
/// </summary>
public interface IPaypalInternalApi
{
    /// <summary>
    /// Internal api
    /// </summary>
    Task<Order> OrdersCreateAsync(OrderRequest request);
}
