using AutoMapper;
using BookVerseApi.Dtos.Book;
using BookVerseApi.Entities;
using BookVerseApi.Interfaces;

namespace BookVerseApi.Services;

public class BooksService : IBooksService
{
    private readonly IBookRepository _repository;
    private readonly IMapper _mapper;

    public BooksService(IBookRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BookReadDto>> GetAllAsync()
    {
        var books = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<BookReadDto>>(books);
    }

    public async Task<BookReadDto?> GetByIdAsync(int id)
    {
        var book = await _repository.GetByIdAsync(id);
        return book == null ? null : _mapper.Map<BookReadDto>(book);
    }

    public async Task<BookReadDto> CreateAsync(BookCreateDto bookDto)
    {
        var book = _mapper.Map<Book>(bookDto);
        var existingBook = await _repository.GetExistingBook(book);
        if (existingBook != null)
        {
            return _mapper.Map<BookReadDto>(existingBook);
        }
        else
        {
            book.CreatedAtUtc = DateTime.UtcNow;
            book.UpdatedAtUtc = DateTime.UtcNow;
            await _repository.AddAsync(book);
            await _repository.FindAsync(b => b.Title == book.Title);
            return _mapper.Map<BookReadDto>(book);
        }
        
    }

    public async Task<bool> UpdateAsync(int id, BookUpdateDto bookDto)
    {
        var book = await _repository.GetByIdAsync(id);
        if (book == null) return false;

        _mapper.Map(bookDto, book);
        book.UpdatedAtUtc = DateTime.UtcNow;
        await _repository.SaveAsync();
        return true;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var book = await _repository.GetByIdAsync(id);
        if (book == null) return false;

        _repository.Delete(book);
        await _repository.SaveAsync();
        return true;
    }
}