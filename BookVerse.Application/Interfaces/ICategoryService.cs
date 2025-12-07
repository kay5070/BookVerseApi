using BookVerse.Application.Dtos.Category;
using BookVerse.Core.Models;

namespace BookVerse.Application.Interfaces;

public interface ICategoryService
{
    Task<PagedResult<CategoryListDto>> GetPagedAsync(QueryParameters parameters);
    Task<IEnumerable<CategoryListDto>> GetAllAsync();
    Task<CategoryReadDto?> GetByIdAsync(int id);
    Task<CategoryReadDto> CreateAsync(CategoryCreateDto categoryDto);
    Task<bool> UpdateAsync(int id, CategoryUpdateDto categoryDto);
    Task<bool> DeleteAsync(int id);
}