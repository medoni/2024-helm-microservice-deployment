namespace POS.Domains.Customer.UseCases.Carts.CartUseCase.Dtos;

/// <summary>
/// Dto for adding a new item to the cart
/// </summary>
/// <param name="MenuItemId">The menu item to add.</param>
/// <param name="RequestedAt">Date and time when the change was requested.</param>
/// <param name="Quantity">Quantity to add.</param>
public record AddItemDto
(
    Guid MenuItemId,
    DateTimeOffset RequestedAt,
    int Quantity
);
