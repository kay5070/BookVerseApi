using BookVerse.Application.Dtos.Category;
using BookVerse.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookVerseApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CategoryController:ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService  categoryService)
    {
        _categoryService = categoryService;
    }
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CategoriesReadDto>>> GetCategories()
    {
        var categories = await _categoryService.GetAllAsync();
        return Ok(categories);
    }
    
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryReadDto>> GetCategoryById(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        return category == null ? NotFound() : Ok(category);
    }
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<CategoryReadDto>> CreateCategory(CategoryCreateDto categoryDto)
    {
        var createdCategory = await _categoryService.CreateAsync(categoryDto);
        return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, createdCategory);
    }
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateDto categoryDto)
    {
        return await _categoryService.UpdateAsync(id, categoryDto) ? NoContent() : NotFound();
    }
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        return await _categoryService.DeleteAsync(id) ? NoContent() : NotFound();
    }
}