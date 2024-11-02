using POS.Domains.Customer.UseCases.CRUDMenuUseCase.Dtos;

namespace POS.Domains.Customer.UseCases.CRUDMenuUseCase;

public interface ICRUDMenuUseCase
{
    Task CreateMenuAsync(CreateMenuDto dto);
    Task UpdateMenuAsync(UpdateMenuDto dto);

    IAsyncEnumerable<MenuDto> GetAllAsync();
    Task<MenuDto> GetByIdAsync(Guid id);
}
