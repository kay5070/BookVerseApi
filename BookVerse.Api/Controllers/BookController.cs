using BookVerse.Application.Dtos.Book;
using BookVerse.Application.Dtos.User;
using BookVerse.Application.Interfaces;
using BookVerse.Core.Constants;
using BookVerse.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookVerse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BookController : ControllerBase
{
    private readonly IBooksService _booksService;

    public BookController(IBooksService booksService)
    {
        _booksService = booksService;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResult<BookReadDto>),StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBooks([FromQuery] BookQueryParameters parameters)
    {
        var books = await _booksService.GetPagedAsync(parameters);
        return Ok(books);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BookReadDto),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookReadDto>> GetBookById(int id)
    {
        if (id <= 0)
            return BadRequest(new BasicResponse 
            { 
                Succeeded = false, 
                Message = ErrorMessages.InvalidId 
            });
        
        var book = await _booksService.GetByIdAsync(id);

        if (book == null)
            return NotFound(new BasicResponse
            {
                Succeeded = false,
                Message = ErrorMessages.BookNotFound
            });
        return Ok(book);
    }

    [HttpPost]
    [Authorize(Roles = IdentityRoleConstants.Admin)]
    [ProducesResponseType(typeof(BookReadDto),StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<BookReadDto>> CreateBook([FromBody]BookCreateDto bookDto)
    {
        if (!ModelState.IsValid)
        {
            var errorMessage = string.Join("; ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));

            return BadRequest(new BasicResponse
            {
                Succeeded = false,
                Message = errorMessage
            });
        }
        var createdBook = await _booksService.CreateAsync(bookDto);
        return CreatedAtAction(nameof(GetBookById), new { id = createdBook.Id }, createdBook);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = IdentityRoleConstants.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateBook(int id, [FromBody] BookUpdateDto bookDto)
    {
        if (id <= 0)
            return BadRequest(new BasicResponse 
            { 
                Succeeded = false, 
                Message = ErrorMessages.InvalidId 
            });
        
        if (!ModelState.IsValid)
        {
            var errorMessage = string.Join("; ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));

            return BadRequest(new BasicResponse
            {
                Succeeded = false,
                Message = errorMessage
            });
        }

        var updated = await _booksService.UpdateAsync(id, bookDto);

        if (!updated)
        {
            return NotFound(new BasicResponse
            {
                Succeeded = false,
                Message = ErrorMessages.BookNotFound
            });
        }
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = IdentityRoleConstants.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBook(int id)
    {
        if (id <= 0)
            return BadRequest(new BasicResponse 
            { 
                Succeeded = false, 
                Message = ErrorMessages.InvalidId 
            });
        var deleted = await _booksService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new BasicResponse
            {
                Succeeded = false,
                Message = ErrorMessages.BookNotFound
            });
        }

        return NoContent();
    }
}