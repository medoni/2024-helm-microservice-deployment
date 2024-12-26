using POS.Domains.Payment.Service.Domain;
using PaypalOrder = PaypalServerSdk.Standard.Models.Order;

namespace POS.Domains.Payment.Service.Processors.Paypal;
internal class PaypalPaymentProviderState : PaymentProviderState
{
    public required PaypalOrder PaypalOrder { get; init; }
}
