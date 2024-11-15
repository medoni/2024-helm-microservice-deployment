using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.Domains.Customer.UseCases.CRUDMenuUseCase;
using POS.Domains.Customer.UseCases.CRUDMenuUseCase.Dtos;
using POS.Domains.Customer.UseCases.PublishMenuUseCase;
using POS.Shared.Infrastructure.Api.Dtos;

namespace POS.Domains.Customer.Api.Controller;

/// <summary>
/// Responsible for handling REST-Apis regarding Menus for Customer
/// </summary>
[ApiVersion(1)]
[ApiController]
[Route("v1/[controller]")]
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
    /// Retrieve a specific menu by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier (UUID) of the menu.</param>
    /// <response code="404">The menu was not found.</response>
    [HttpGet("{id}")]
    [ProducesResponseType<MenuDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/json")]
    public async Task<IActionResult> GetMenuByIdAsync(Guid id)
    {
        var menu = await _crudMenuUseCase.GetByIdAsync(id);

        return Ok(menu);
    }

    /// <summary>
    /// Create a new menu with sections and items.
    /// </summary>
    /// <response code="201">The menu was successfully created.</response>
    /// <response code="400">The menu or it's content was incorrect.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/json")]
    public async Task<IActionResult> CreateMenuAsync(CreateMenuDto dto)
    {
        await _crudMenuUseCase.CreateMenuAsync(dto);
        return Created();
    }

    /// <summary>
    /// Retrieve all menus available in the system.
    /// </summary>
    [HttpGet]
    [ProducesResponseType<ResultSetDto<MenuDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMenusAsync(
        string? token = null
    )
    {
        var result = await _crudMenuUseCase.GetAllAsync().ToArrayAsync();

        return Ok(result);
    }

    /// <summary>
    /// Update an existing menu.
    /// </summary>
    /// <response code="204">The menu was successfully updated.</response>
    /// <response code="400">The menu or it's content was incorrect.</response>
    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/json")]
    public async Task<IActionResult> UpdateMenuAsync(UpdateMenuDto dto)
    {
        await _crudMenuUseCase.UpdateMenuAsync(dto);
        return NoContent();
    }

    /// <summary>
    /// Publish an existing menu to make it visible to customers.
    /// </summary>
    /// <param name="id">The unique identifier (UUID) of the menu to publish.</param>
    /// <response code="204">The menu was successfully published.</response>
    /// <response code="400">The menu or the state is incorrect.</response>
    /// <response code="404">The menu was not found.</response>
    [HttpPatch("{id}/publish")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, "application/json")]
    public async Task<IActionResult> PublishMenuAsync(Guid id)
    {
        await _publishMenuUseCase.PublishAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Retrieve the currently active menu.
    /// </summary>
    [HttpGet("active")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActiveMenuAsync()
    {
        var menu = await _publishMenuUseCase.GetActiveAsync();

        return Ok(menu);
    }
}
