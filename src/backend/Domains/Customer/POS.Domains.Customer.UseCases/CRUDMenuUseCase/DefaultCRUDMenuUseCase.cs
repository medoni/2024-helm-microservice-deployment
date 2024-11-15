using POS.Domains.Customer.Domain.Menus;
using POS.Domains.Customer.Persistence.Menus;
using POS.Domains.Customer.UseCases.CRUDMenuUseCase.Dtos;
using POS.Domains.Customer.UseCases.CRUDMenuUseCase.Mapper;
using POS.Shared.Persistence.UOW;

namespace POS.Domains.Customer.UseCases.CRUDMenuUseCase;

internal class DefaultCRUDMenuUseCase : ICRUDMenuUseCase
{
    private readonly UnitOfWorkFactory _uowFactory;
    private readonly IMenuRespository _menuRepository;

    public DefaultCRUDMenuUseCase(
        UnitOfWorkFactory uowFactory,
        IMenuRespository menuRepository
    )
    {
        _uowFactory = uowFactory ?? throw new ArgumentNullException(nameof(uowFactory));
        _menuRepository = menuRepository ?? throw new ArgumentNullException(nameof(menuRepository));
    }

    public async Task CreateMenuAsync(CreateMenuDto dto)
    {
        var uow = _uowFactory();

        var menu = new Menu(
            dto.Id,
            DateTimeOffset.UtcNow,
            dto.Sections.ToEntities()
        );

        uow.Add(menu);

        await uow.CommitAsync();
    }

    public async IAsyncEnumerable<MenuDto> GetAllAsync()
    {
        await foreach (var menu in _menuRepository.IterateAsync())
        {
            yield return menu.ToDto();
        }
    }

    public async Task<MenuDto> GetByIdAsync(Guid id)
    {
        var uow = _uowFactory();

        var menu = await uow.GetAsync<Menu>(id);
        return menu.ToDto();
    }

    public async Task UpdateMenuAsync(UpdateMenuDto dto)
    {
        var uow = _uowFactory();
        var menu = await uow.GetAsync<Menu>(dto.Id);

        menu.UpdateSections(
            DateTimeOffset.UtcNow,
            dto.Sections.ToEntities()
        );

        await uow.CommitAsync();
    }
}
