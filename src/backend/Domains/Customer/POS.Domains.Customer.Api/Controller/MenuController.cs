using Microsoft.AspNetCore.Mvc;
using POS.Domains.Customer.UseCases.CRUDMenuUseCase;
using POS.Domains.Customer.UseCases.CRUDMenuUseCase.Dtos;
using POS.Domains.Customer.UseCases.PublishMenuUseCase;

namespace POS.Domains.Customer.Api.Controller;

/// <summary>
/// Responsible for handling REST-Apis regarding Menus for Customer
/// </summary>
[ApiController]
[Route("[controller]")]
public class MenuController : ControllerBase
{
    private readonly ICRUDMenuUseCase _crudMenuUseCase;
    private readonly IPublishMenuUseCase _publishMenuUseCase;

    /// <summary>
    /// Creates a new <see cref="MenuController"/>
    /// </summary>
    public MenuController(ICRUDMenuUseCase crudMenuUseCase, IPublishMenuUseCase publishMenuUseCase)
    {
        _crudMenuUseCase = crudMenuUseCase ?? throw new ArgumentNullException(nameof(crudMenuUseCase));
        _publishMenuUseCase = publishMenuUseCase ?? throw new ArgumentNullException(nameof(publishMenuUseCase));
    }

    /// <summary>
    /// Returns a MenuDto by a given Menu-Id
    /// </summary>
    /// <param name="id">The id of the menu</param>
    [HttpGet("{id}")]
    [ProducesResponseType<MenuDto>(200)]
    public async Task<IActionResult> GetMenuByIdAsync(Guid id)
    {
        var menu = await _crudMenuUseCase.GetByIdAsync(id);

        return Ok(menu);
    }

    /// <summary>
    /// Creates a new Menu
    /// </summary>
    /// <param name="dto">The menu to create</param>
    [HttpPost]
    [ProducesResponseType(200)]
    public async Task<IActionResult> CreateMenuAsync(CreateMenuDto dto)
    {
        await _crudMenuUseCase.CreateMenuAsync(dto);
        return Ok();
    }

    /// <summary>
    /// Updates an existing Menu
    /// </summary>
    /// <param name="dto">The menu to update</param>
    [HttpPatch]
    [ProducesResponseType(200)]
    public async Task<IActionResult> UpdateMenuAsync(UpdateMenuDto dto)
    {
        await _crudMenuUseCase.UpdateMenuAsync(dto);
        return Ok();
    }

    /// <summary>
    /// Returns all Menus
    /// </summary>
    [HttpGet]
    [ProducesResponseType<List<MenuDto>>(200)]
    public async Task<IActionResult> GetMenusAsync()
    {
        var result = await _crudMenuUseCase.GetAllAsync().ToArrayAsync();

        return Ok(result);
    }

    /// <summary>
    /// Publishes an existing menu.
    /// </summary>
    [HttpPatch("{id}/publish")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> PublishMenuAsync(Guid id)
    {
        await _publishMenuUseCase.PublishAsync(id);
        return Ok();
    }

    /// <summary>
    /// Returns the active menu
    /// </summary>
    [HttpGet("active")]
    [ProducesResponseType(200)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> GetActiveMenuAsync()
    {
        var menu = await _publishMenuUseCase.GetActiveAsync();

        return Ok(menu);
    }
}
