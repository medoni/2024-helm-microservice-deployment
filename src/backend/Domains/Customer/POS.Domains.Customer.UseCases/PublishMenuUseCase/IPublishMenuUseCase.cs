using POS.Domains.Customer.UseCases.CRUDMenuUseCase.Dtos;

namespace POS.Domains.Customer.UseCases.PublishMenuUseCase;
public interface IPublishMenuUseCase
{
    Task PublishAsync(Guid id);
    Task<MenuDto?> GetActiveAsync();
}
