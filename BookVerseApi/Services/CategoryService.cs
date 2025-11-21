using AutoMapper;
using BookVerseApi.Dtos.Category;
using BookVerseApi.Entities;
using BookVerseApi.Interfaces;

namespace BookVerseApi.Services;

public class CategoryService: ICategoryService
{
    private readonly ICategoryRepository _repository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoriesReadDto>> GetAllAsync()
    {
        var categories = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<CategoriesReadDto>>(categories);
    }

    public async Task<CategoryReadDto?> GetByIdAsync(int id)
    {
        var category = await _repository.GetByIdAsync(id);
        return category == null ? null : _mapper.Map<CategoryReadDto>(category);
    }



    public async Task<CategoryReadDto> CreateAsync(CategoryCreateDto categoryDto)
    {
        var category = _mapper.Map<Category>(categoryDto);
        var existingCategory = await _repository.GetByNameAsync(category.Name);
        if (existingCategory != null)
        {
            return _mapper.Map<CategoryReadDto>(existingCategory);
        }
        else
        {
            await _repository.AddAsync(category);
            await _repository.SaveAsync();
            return _mapper.Map<CategoryReadDto>(category);
        }
    }

    public async Task<bool> UpdateAsync(int id, CategoryUpdateDto categoryDto)
    {
        var category = await _repository.GetByIdAsync(id);
        if (category == null) return false;

        _mapper.Map(categoryDto, category);
        await _repository.SaveAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category = await _repository.GetByIdAsync(id);
        if (category == null) return false;
        _repository.Delete(category);
        await _repository.SaveAsync();
        return true;
    }
}