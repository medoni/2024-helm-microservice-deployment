namespace POS.Domains.Customer.UseCases.Carts.CartUseCase.Dtos;

/// <summary>
/// Dto when a cart was checkout out.
/// </summary>
public record CartCheckedOutDto
(
    Guid CartId,
    Guid OrderId,
    DateTimeOffset CheckedOutAt
);
