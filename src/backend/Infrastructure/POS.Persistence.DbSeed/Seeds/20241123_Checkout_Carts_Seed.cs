using POS.Domains.Customer.Persistence.Carts;
using POS.Domains.Customer.UseCases.Carts.CartUseCase;
using POS.Shared.Persistence.PostgreSql.DbSeeds;

namespace POS.Persistence.DbSeed.Seeds;
internal class _20241123_Checkout_Carts_Seed : ISeeder
{
    private readonly ICartUseCase _cartsService;
    private readonly ICartRepository _cartRepo;

    private static Random Random = Random.Shared;

    public _20241123_Checkout_Carts_Seed(
        ICartUseCase cartsService,
        ICartRepository cartRepo
    )
    {
        _cartsService = cartsService ?? throw new ArgumentNullException(nameof(cartsService));
        _cartRepo = cartRepo ?? throw new ArgumentNullException(nameof(cartRepo));
    }

    public string Name => "Checkout carts seed";

    public DateTimeOffset AddedAt => new DateTime(2011, 11, 23, 07, 7, 00, DateTimeKind.Utc);

    public async Task SeedAsync()
    {
        var cartIds = await GetCartIdsToCheckoutAsync();
        foreach (var cartId in cartIds)
        {
            await _cartsService.CheckoutCartAsync(cartId, new(DateTimeOffset.UtcNow));
        }
    }

    private async Task<IReadOnlyList<Guid>> GetCartIdsToCheckoutAsync()
    {
        var cartIds = await _cartRepo.IterateAsync()
            .Where(x => Random.Next(1, 3) > 1)
            .Select(x => x.Id)
            .ToArrayAsync();

        return cartIds;
    }
}
