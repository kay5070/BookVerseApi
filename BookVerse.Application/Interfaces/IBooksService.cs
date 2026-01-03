using BookVerse.Application.Dtos.Book;
using BookVerse.Core.Models;

namespace BookVerse.Application.Interfaces;

public interface IBooksService
{
    Task<PagedResult<BookReadDto>> GetPagedAsync(BookQueryParameters parameters);
    Task<BookReadDto?> GetByIdAsync(int id);
    Task<BookReadDto> CreateAsync(BookCreateDto bookDto);
    Task<bool> UpdateAsync(int id, BookUpdateDto bookDto);
    Task<bool> DeleteAsync(int id);
}