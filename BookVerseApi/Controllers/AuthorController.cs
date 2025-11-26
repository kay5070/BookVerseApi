using BookVerse.Application.Dtos.Author;
using BookVerse.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookVerseApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthorController:ControllerBase
{
    private readonly IAuthorsService _service;

    public AuthorController(IAuthorsService service)
    {
        _service = service;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AuthorReadDto>>> GetAuthors()
    {
        var authors = await _service.GetAllAsync();
        return Ok(authors);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AuthorReadDto>> GetAuthor(int id)
    {
        var author = await _service.GetByIdAsync(id);
        return Ok(author);
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<AuthorReadDto>> CreateAuthor(AuthorCreateDto authorDto)
    {
        return await _service.CreateAsync(authorDto);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAuthor(int id, AuthorUpdateDto authorDto)
    {
        return await _service.UpdateAsync(id, authorDto) ? NoContent() : NotFound();
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        return await _service.DeleteAsync(id) ? NoContent():NotFound();
    }
}