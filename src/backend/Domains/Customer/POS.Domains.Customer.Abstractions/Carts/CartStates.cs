namespace POS.Domains.Customer.Abstractions.Carts;

/// <summary>
/// States of a cart
/// </summary>
public enum CartStates
{
    /// <summary>
    /// Cart was created.
    /// </summary>
    Created,

    /// <summary>
    /// Cart was checked out.
    /// </summary>
    CheckedOut
}
