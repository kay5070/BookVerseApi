using BookVerse.Application.Dtos.Book;

namespace BookVerse.Application.Interfaces;

public interface IBooksService
{
    Task<IEnumerable<BookReadDto>> GetAllAsync();
    Task<BookReadDto?> GetByIdAsync(int id);
    Task<BookReadDto> CreateAsync(BookCreateDto bookDto);
    Task<bool> UpdateAsync(int id, BookUpdateDto bookDto);
    Task<bool> DeleteAsync(int id);
}