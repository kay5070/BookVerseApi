using BookVerse.Application.Dtos.Category;
using BookVerse.Application.Dtos.User;
using BookVerse.Application.Interfaces;
using BookVerse.Core.Constants;
using BookVerse.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookVerseApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResult<CategoryListDto>),StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategories([FromQuery] QueryParameters parameters)
    {
        var categories = await _categoryService.GetPagedAsync(parameters);
        return Ok(categories);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(CategoryReadDto),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryReadDto>> GetCategoryById(int id)
    {
        if (id <= 0)
            return BadRequest(new BasicResponse 
            { 
                Succeeded = false, 
                Message = ErrorMessages.InvalidId 
            });
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null)
            return NotFound(new BasicResponse 
            { 
                Succeeded = false, 
                Message = ErrorMessages.CategoryNotFound 
            });
        else
            return Ok(category);
    }

    [HttpPost]
    [Authorize(Roles = IdentityRoleConstants.Admin)]
    [ProducesResponseType(typeof(CategoryReadDto),StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<CategoryReadDto>> CreateCategory(CategoryCreateDto categoryDto)
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
        var createdCategory = await _categoryService.CreateAsync(categoryDto);
        return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, createdCategory);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = IdentityRoleConstants.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCategory(int id, CategoryUpdateDto categoryDto)
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
        var updated = await _categoryService.UpdateAsync(id, categoryDto);
        if (!updated)
        {
            return NotFound(new BasicResponse
            {
                Succeeded = false,
                Message = ErrorMessages.CategoryNotFound
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
    public async Task<IActionResult> DeleteCategory(int id)
    {
        if (id <= 0)
            return BadRequest(new BasicResponse 
            { 
                Succeeded = false, 
                Message = ErrorMessages.InvalidId 
            });
        var deleted = await _categoryService.DeleteAsync(id);

        if (!deleted)
            return NotFound(new BasicResponse 
            { 
                Succeeded = false, 
                Message = ErrorMessages.CategoryNotFound 
            });

        return NoContent();
    }
}