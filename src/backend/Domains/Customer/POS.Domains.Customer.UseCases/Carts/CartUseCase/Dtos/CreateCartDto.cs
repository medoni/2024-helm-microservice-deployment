namespace POS.Domains.Customer.UseCases.Carts.CartUseCase.Dtos;

/// <summary>
/// Dto for creating a new cart.
/// </summary>
/// <param name="Id">Id of the cart.</param>
/// <param name="RequestedAt">Date and time when the create was requested.</param>
public record CreateCartDto
(
    Guid Id,
    DateTimeOffset RequestedAt
);
