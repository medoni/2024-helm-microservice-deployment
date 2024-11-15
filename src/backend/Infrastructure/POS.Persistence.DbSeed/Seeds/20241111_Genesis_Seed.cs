using POS.Domains.Customer.Api.Dtos.Examples;
using POS.Domains.Customer.UseCases.CRUDMenuUseCase;
using POS.Domains.Customer.UseCases.PublishMenuUseCase;
using POS.Shared.Persistence.PostgreSql.DbSeeds;

namespace POS.Persistence.DbSeed.Seeds;
internal class _20241111_Genesis_Seed : ISeeder
{
    private readonly ICRUDMenuUseCase _menuService;
    private readonly IPublishMenuUseCase _menuPublishService;

    public _20241111_Genesis_Seed(
        ICRUDMenuUseCase menuService,
        IPublishMenuUseCase menuPublishService
    )
    {
        _menuService = menuService ?? throw new ArgumentNullException(nameof(menuService));
        _menuPublishService = menuPublishService ?? throw new ArgumentNullException(nameof(menuPublishService));
    }

    public string Name => "Genesis Seeder";

    public DateTimeOffset AddedAt => new DateTime(2011, 11, 11, 10, 40, 00, DateTimeKind.Utc);

    public async Task SeedAsync()
    {
        var menuDto = new CreateMenuDtoExample().GetExamples();

        await _menuService.CreateMenuAsync(menuDto);
        await _menuPublishService.PublishAsync(menuDto.Id);
    }
}
