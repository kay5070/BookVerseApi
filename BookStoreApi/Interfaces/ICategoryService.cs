using BookStoreApi.Dtos.Category;

namespace BookStoreApi.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoriesReadDto>> GetAllAsync();
    Task<CategoryReadDto?> GetByIdAsync(int id);
    Task<CategoryReadDto> CreateAsync(CategoryCreateDto categoryDto);
    Task<bool> UpdateAsync(int id, CategoryUpdateDto categoryDto);
    Task<bool> DeleteAsync(int id);
}