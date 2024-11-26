using POS.Domains.Customer.UseCases.Menus.CRUDMenuUseCase.Dtos;
using POS.Shared.Domain.Generic.Dtos;

namespace POS.Domains.Customer.UseCases.Carts.CartUseCase.Dtos;

/// <summary>
/// Dto for cart item.
/// </summary>
/// <param name="Id">Unique id of the cart item. Same as <see cref="MenuItemDto.Id"/>.</param>
/// <param name="AddedAt">Date and time when the item was added.</param>
/// <param name="Name">Name of the item.</param>
/// <param name="Description">Description of the item.</param>
/// <param name="UnitPrice">Unit price of the item.</param>
/// <param name="Quantity">Quantity.</param>
public record CartItemDto
(
    Guid Id,
    DateTimeOffset AddedAt,
    string Name,
    string Description,
    PriceInfoDto UnitPrice,
    int Quantity
)
{
}
