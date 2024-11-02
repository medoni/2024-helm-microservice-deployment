using POS.Domains.Customer.Domain.Menus;
using POS.Domains.Customer.Persistence.Menus;
using POS.Domains.Customer.UseCases.CRUDMenuUseCase.Dtos;
using POS.Domains.Customer.UseCases.CRUDMenuUseCase.Mapper;
using POS.Shared.Persistence.UOW;

namespace POS.Domains.Customer.UseCases.PublishMenuUseCase;
internal class DefaultPublishMenuUseCase : IPublishMenuUseCase
{
    private readonly UnitOfWorkFactory _uowFactory;
    private readonly IMenuRespository _menuRepo;

    public DefaultPublishMenuUseCase(
        UnitOfWorkFactory uowFactory,
        IMenuRespository menuRepo
    )
    {
        _uowFactory = uowFactory ?? throw new ArgumentNullException(nameof(uowFactory));
        _menuRepo = menuRepo ?? throw new ArgumentNullException(nameof(menuRepo));
    }

    public async Task<MenuDto?> GetActiveAsync()
    {
        var activeMenu = await _menuRepo.GetActiveAsync();
        if (activeMenu is null) return null;

        return activeMenu.ToDto();
    }

    public async Task PublishAsync(Guid id)
    {
        var utcNow = DateTimeOffset.UtcNow;
        var uow = _uowFactory();

        var activeMenuId = (await _menuRepo.GetActiveAsync())?.Id;

        if (activeMenuId != null)
        {
            var activeMenu = await uow.GetAsync<Menu>(activeMenuId.Value);
            activeMenu.Deactivate(
                utcNow
            );
        }

        var newActiveMenu = await uow.GetAsync<Menu>(id);
        newActiveMenu.Activate(utcNow);

        await uow.CommitAsync();
    }
}
