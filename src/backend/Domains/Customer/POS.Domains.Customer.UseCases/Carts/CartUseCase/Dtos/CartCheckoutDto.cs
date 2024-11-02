namespace POS.Domains.Customer.UseCases.Carts.CartUseCase.Dtos;

/// <summary>
/// Dto when a cart gets checkout
/// </summary>
public record CartCheckOutDto
(
    DateTimeOffset CheckoutAt
);
