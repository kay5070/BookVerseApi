using BookVerse.Application.Dtos.Author;
using BookVerse.Core.Models;

namespace BookVerse.Application.Interfaces;

public interface IAuthorsService
{
    Task<PagedResult<AuthorListDto>> GetPagedAsync(QueryParameters parameters);
    Task<AuthorReadDto?> GetByIdAsync(int id);
    Task<AuthorReadDto> CreateAsync(AuthorCreateDto authorDto);
    Task<bool> UpdateAsync(int id, AuthorUpdateDto authorDto);
    Task<bool> DeleteAsync(int id);
}