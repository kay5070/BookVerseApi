using AutoMapper;
using BookVerse.Application.Dtos.Category;
using BookVerse.Application.Interfaces;
using BookVerse.Core.Entities;
using BookVerse.Core.Models;
using Microsoft.Extensions.Logging;

namespace BookVerse.Infrastructure.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CategoryService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedResult<CategoryListDto>> GetPagedAsync(QueryParameters parameters)
    {
        var pagedCategories = await _unitOfWork.Categories.GetPagedAsync(parameters);
        var categoryDtos = _mapper.Map<IEnumerable<CategoryListDto>>(pagedCategories.Items);

        return new PagedResult<CategoryListDto>(
            categoryDtos,
            pagedCategories.TotalCount,
            pagedCategories.CurrentPage,
            pagedCategories.PageSize
        );
    }

    public async Task<IEnumerable<CategoryListDto>> GetAllAsync()
    {
        try
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<CategoryListDto>>(categories);

            _logger.LogInformation("Retrieved {Count} categories", dtos.Count());

            return dtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all categories");
            return Enumerable.Empty<CategoryListDto>();
        }
    }

    public async Task<CategoryReadDto?> GetByIdAsync(int id)
    {
        try
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
            {
                _logger.LogWarning("Category not found with ID: {CategoryId}", id);
                return null;
            }

            var dto = _mapper.Map<CategoryReadDto>(category);
            _logger.LogInformation("Retrieved category: {CategoryId}", id);

            return dto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving category with ID: {CategoryId}", id);
            return null;
        }
    }


    public async Task<CategoryReadDto> CreateAsync(CategoryCreateDto categoryDto)
    {
        try
        {
            var category = _mapper.Map<Category>(categoryDto);
            var existingCategory = await _unitOfWork.Categories.GetByNameAsync(category.Name);
            
            if (existingCategory != null)
            {
                _logger.LogInformation("Category already exists: {CategoryName}", category.Name);
                
                return _mapper.Map<CategoryReadDto>(existingCategory);
            }

            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();
            
            _logger.LogInformation("Created new category: {CategoryName}", category.Name);
            return _mapper.Map<CategoryReadDto>(category);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating category: {CategoryName}", categoryDto.Name);
            throw;
        }
    }

    public async Task<bool> UpdateAsync(int id, CategoryUpdateDto categoryDto)
    {
        try
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
            {
                _logger.LogWarning("Attempted to update non-existent category with ID: {CategoryId}", id);
                return false;
            }

            _mapper.Map(categoryDto, category);
            _unitOfWork.Categories.Update(category);
            await _unitOfWork.SaveChangesAsync();
            
            _logger.LogInformation("Updated category: {CategoryId}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating category: {CategoryId}", id);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
            {
                _logger.LogWarning("Attempted to delete non-existent category with ID: {CategoryId}", id);
                return false;
            }
            _unitOfWork.Categories.Delete(category);
            await _unitOfWork.SaveChangesAsync();
            
            _logger.LogInformation("Deleted category: {CategoryId}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting category: {CategoryId}", id);
            throw;
        }
    }
}