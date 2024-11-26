namespace POS.Domains.Customer.UseCases.Carts.CartUseCase.Dtos;

/// <summary>
/// Dto for updating an existing cart item
/// </summary>
/// <param name="MenuItemId">The menu item to update.</param>
/// <param name="RequestedAt">Date and time when the change was requested.</param>
/// <param name="Quantity">The new quantity.</param>
public record UpdateItemDto
(
    Guid MenuItemId,
    DateTimeOffset RequestedAt,
    int Quantity
);
