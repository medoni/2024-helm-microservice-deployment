using POS.Domains.Customer.Abstractions.Carts;
using POS.Shared.Domain.Generic.Dtos;

namespace POS.Domains.Customer.UseCases.Carts.CartUseCase.Dtos;

/// <summary>
/// Dto for cart created by the customer.
/// </summary>
public record CartDto
(
    Guid Id,
    DateTimeOffset CreatedAt,
    DateTimeOffset LastChangedAt,
    CartStates State,
    Guid ActiveMenuId,
    int TotalItems,
    GrossNetPriceDto TotalPrice,
    CartCheckoutInfoDto? CheckoutInfo
);
