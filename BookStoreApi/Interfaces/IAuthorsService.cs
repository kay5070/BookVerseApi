using BookStoreApi.Dtos.Author;

namespace BookStoreApi.Interfaces;

public interface IAuthorsService
{
    Task<IEnumerable<AuthorsReadDto>> GetAllAsync();
    Task<AuthorReadDto?> GetByIdAsync(int id);
    Task<AuthorReadDto> CreateAsync(AuthorCreateDto authorDto);
    Task<bool> UpdateAsync(int id, AuthorUpdateDto authorDto);
    Task<bool> DeleteAsync(int id);
}