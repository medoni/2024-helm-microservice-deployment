using Microsoft.Extensions.Logging;
using POS.Domains.Customer.UseCases.Carts.CartUseCase.Dtos;
using POS.Shared.Infrastructure.Api.Dtos;

namespace POS.Domains.Customer.UseCases.Carts.CartUseCase;
internal class LoggingCartUseCaseDecorator
(
    ICartUseCase next,
    ILogger<ICartUseCase> logger
) : ICartUseCase
{
    public async Task CreateCartAsync(CreateCartDto dto)
    {
        try
        {
            logger.LogInformation("Creating a new cart with name '{cartId}'...", dto.Id);

            await next.CreateCartAsync(dto);

            logger.LogInformation("Successfully created a new cart with name '{cartId}'.", dto.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating a new cart with name '{cartId}'.", dto.Id);
            throw;
        }
    }

    public async Task<CartDto> GetCartByIdAsync(Guid id)
    {
        try
        {
            logger.LogInformation("Getting cart with id '{cartId}'...", id);

            var cart = await next.GetCartByIdAsync(id);

            logger.LogInformation("Successfully retrieved cart with id '{cartId}'.", id);

            return cart;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting cart with id '{cartId}'.", id);
            throw;
        }
    }

    public async Task<CartItemDto?> AddItemToCartAsync(Guid id, AddItemDto dto)
    {
        try
        {
            logger.LogInformation("Adding item '{menuItemId}' to cart with id '{cartId}'...", dto.MenuItemId, id);

            var result = await next.AddItemToCartAsync(id, dto);

            logger.LogInformation("Successfully added item '{menuItemId}' to cart with id '{cartId}'.", dto.MenuItemId, id);

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error adding item '{menuItemId}' to cart with id '{cartId}'.", dto.MenuItemId, id);
            throw;
        }
    }

    public async Task<ResultSetDto<CartItemDto>> GetCartItemsAsync(Guid id, string? token = null)
    {
        try
        {
            logger.LogInformation("Getting items for cart with id '{cartId}' (token: '{token}')...", id, token);

            var items = await next.GetCartItemsAsync(id, token);

            logger.LogInformation("Successfully retrieved items for cart with id '{cartId}' (token: '{token}').", id, token);

            return items;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting items for cart with id '{cartId}' (token: '{token}').", id, token);
            throw;
        }
    }

    public async Task<CartItemDto?> UpdateCartItemAsync(Guid id, UpdateItemDto dto)
    {
        try
        {
            logger.LogInformation("Updating item '{menuItemId}' in cart with id '{cartId}'...", dto.MenuItemId, id);

            var result = await next.UpdateCartItemAsync(id, dto);

            logger.LogInformation("Successfully updated item '{menuItemId}' in cart with id '{cartId}'.", dto.MenuItemId, id);

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating item '{menuItemId}' in cart with id '{cartId}'.", dto.MenuItemId, id);
            throw;
        }
    }

    public async Task<CartCheckedOutDto> CheckoutCartAsync(Guid cartId, CartCheckOutDto dto)
    {
        try
        {
            logger.LogInformation("Checking out cart with id '{cartId}'...", cartId);

            var result = await next.CheckoutCartAsync(cartId, dto);

            logger.LogInformation("Successfully checked out cart with id '{cartId}'.", cartId);

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error checking out cart with id '{cartId}'.", cartId);
            throw;
        }
    }
}
