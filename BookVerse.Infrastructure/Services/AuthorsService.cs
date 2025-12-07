using AutoMapper;
using BookVerse.Application.Dtos.Author;
using BookVerse.Application.Interfaces;
using BookVerse.Core.Entities;
using BookVerse.Core.Models;
using Microsoft.Extensions.Logging;

namespace BookVerse.Infrastructure.Services;

public class AuthorsService : IAuthorsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<AuthorsService> _logger;

    public AuthorsService(IUnitOfWork unitOfWork, IMapper mapper,ILogger<AuthorsService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedResult<AuthorListDto>> GetPagedAsync(QueryParameters parameters)
    {
        var pagedAuthors = await _unitOfWork.Authors.GetPagedAsync(parameters);
        var authorDtos = _mapper.Map<IEnumerable<AuthorListDto>>(pagedAuthors.Items);

        return new PagedResult<AuthorListDto>(
            authorDtos,
            pagedAuthors.TotalCount,
            pagedAuthors.CurrentPage,
            pagedAuthors.PageSize
        );
    }

    public async Task<IEnumerable<AuthorListDto>> GetAllAsync()
    {
        try
        {
            var authors = await _unitOfWork.Authors.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<AuthorListDto>>(authors);
        
            _logger.LogInformation("Retrieved {Count} authors", dtos.Count());
        
            return dtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all authors");
            return Enumerable.Empty<AuthorListDto>(); 
        }
    }

    public async Task<AuthorReadDto?> GetByIdAsync(int id)
    {
        try
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(id);
            if (author == null)
            {
                _logger.LogWarning("Author not found with ID: {AuthorId}", id);
                return null;
            }

            var dto = _mapper.Map<AuthorReadDto>(author);
            _logger.LogInformation("Retrieved author: {AuthorId}", id);
            return dto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving author: {AuthorId}", id);
            return null; // no throw
        }
    }

    public async Task<AuthorReadDto> CreateAsync(AuthorCreateDto authorDto)
    {
        try
        {
            var author = _mapper.Map<Author>(authorDto);
            var existingAuthor = await _unitOfWork.Authors.GetByNameAsync(author.FirstName, author.LastName);

            if (existingAuthor != null)
            {
                _logger.LogInformation("Author already exists: {FirstName} {LastName}", 
                    author.FirstName, author.LastName);
                return _mapper.Map<AuthorReadDto>(existingAuthor);
            }

            await _unitOfWork.Authors.AddAsync(author);
            await _unitOfWork.SaveChangesAsync();
            
            _logger.LogInformation("Created new author: {FirstName} {LastName}", 
                author.FirstName, author.LastName);
            
            return _mapper.Map<AuthorReadDto>(author);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating author: {FirstName} {LastName}", 
                authorDto.FirstName, authorDto.LastName);
            throw;
        }
    }

    public async Task<bool> UpdateAsync(int id, AuthorUpdateDto authorDto)
    {
        try
        {
            var retrievedAuthor = await _unitOfWork.Authors.GetByIdAsync(id);
            if (retrievedAuthor == null)
            {
                _logger.LogWarning("Attempted to update non-existent author with ID: {AuthorId}", id);
                return false;
            }

            _mapper.Map(authorDto, retrievedAuthor);
            _unitOfWork.Authors.Update(retrievedAuthor);
            await _unitOfWork.SaveChangesAsync();
            
            _logger.LogInformation("Updated author: {AuthorId}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating author: {AuthorId}", id);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var retrievedAuthor = await _unitOfWork.Authors.GetByIdAsync(id);
            if (retrievedAuthor == null) 
            {
                _logger.LogWarning("Attempted to delete non-existent author with ID: {AuthorId}", id);
                return false;
            }

            _unitOfWork.Authors.Delete(retrievedAuthor);
            await _unitOfWork.SaveChangesAsync();
            
            _logger.LogInformation("Deleted author: {AuthorId}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting author: {AuthorId}", id);
            throw;
        }
    }
}