using Microsoft.AspNetCore.Mvc;
using POS.Domains.Customer.UseCases.CRUDMenuUseCase;
using POS.Domains.Customer.UseCases.CRUDMenuUseCase.Dtos;
using POS.Domains.Customer.UseCases.PublishMenuUseCase;

namespace POS.Domains.Customer.Api.Controller;

[ApiController]
[Route("[controller]")]
public class MenuController : ControllerBase
{
    private readonly ICRUDMenuUseCase _crudMenuUseCase;
    private readonly IPublishMenuUseCase _publishMenuUseCase;

    public MenuController(ICRUDMenuUseCase crudMenuUseCase, IPublishMenuUseCase publishMenuUseCase)
    {
        _crudMenuUseCase = crudMenuUseCase ?? throw new ArgumentNullException(nameof(crudMenuUseCase));
        _publishMenuUseCase = publishMenuUseCase ?? throw new ArgumentNullException(nameof(publishMenuUseCase));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMenuByIdAsync(Guid id)
    {
        var menu = await _crudMenuUseCase.GetByIdAsync(id);

        return Ok(menu);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMenuAsync(CreateMenuDto dto)
    {
        await _crudMenuUseCase.CreateMenuAsync(dto);
        return Ok();
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateMenuAsync(UpdateMenuDto dto)
    {
        await _crudMenuUseCase.UpdateMenuAsync(dto);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetMenusAsync()
    {
        var result = await _crudMenuUseCase.GetAllAsync().ToArrayAsync();

        return Ok(result);
    }

    [HttpPatch("{id}/publish")]
    public async Task<IActionResult> PublishMenuAsync(Guid id)
    {
        await _publishMenuUseCase.PublishAsync(id);
        return Ok();
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActiveMenuAsync()
    {
        var menu = await _publishMenuUseCase.GetActiveAsync();

        return Ok(menu);
    }
}
