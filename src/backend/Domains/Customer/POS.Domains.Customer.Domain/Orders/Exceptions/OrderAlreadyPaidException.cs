namespace POS.Domains.Customer.Domain.Orders.Exceptions;

/// <summary>
/// Exception, raised when a order has already been payed.
/// </summary>
public class OrderAlreadyPaidException : Exception
{
    /// <summary>
    /// Creates a new <see cref="OrderAlreadyPaidException"/>
    /// </summary>
    public OrderAlreadyPaidException(Guid orderId) : base($"The order with id {orderId} has been already paid.") { }
}
