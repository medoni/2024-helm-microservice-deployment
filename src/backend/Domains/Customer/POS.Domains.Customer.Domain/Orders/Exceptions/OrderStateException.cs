using POS.Domains.Customer.Abstractions.Orders;

namespace POS.Domains.Customer.Domain.Orders.Exceptions;
/// <summary>
/// Exception, raised when the state of the order is not expected.
/// </summary>
public class OrderStateException : Exception
{
    /// <summary>
    /// Creates a new <see cref="OrderStateException"/>.
    /// </summary>
    public OrderStateException(
        Guid orderId,
        OrderStates currentState,
        OrderStates expectedState
    ) : base($"The order with id {orderId} is an invalid state. Current state: '{currentState}'. Expected state: '{expectedState}'")
    {
    }
}
