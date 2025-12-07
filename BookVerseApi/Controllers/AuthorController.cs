using BookVerse.Application.Dtos.Author;
using BookVerse.Application.Interfaces;
using BookVerse.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookVerseApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthorController : ControllerBase
{
    private readonly IAuthorsService _service;
    public AuthorController(IAuthorsService service)
    {
        _service = service;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResult<AuthorsReadDto>),StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<AuthorReadDto>>> GetAuthors([FromQuery] QueryParameters parameters)
    {
        var authors = await _service.GetPagedAsync(parameters);
        return Ok(authors);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthorReadDto),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AuthorReadDto>> GetAuthor(int id)
    {
        var author = await _service.GetByIdAsync(id);
        if (author == null) return NotFound();
        return Ok(author);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(AuthorReadDto),StatusCodes.Status201Created)]
    public async Task<ActionResult<AuthorReadDto>> CreateAuthor(AuthorCreateDto authorDto)
    {
        var createdAuthor =await _service.CreateAsync(authorDto);

        return CreatedAtAction(nameof(GetAuthor), new { id = createdAuthor.Id }, createdAuthor);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAuthor(int id, AuthorUpdateDto authorDto)
    {
        return await _service.UpdateAsync(id, authorDto) ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        return await _service.DeleteAsync(id) ? NoContent() : NotFound();
    }
}