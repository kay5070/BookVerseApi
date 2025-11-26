using AutoMapper;
using BookVerse.Application.Dtos.Book;
using BookVerse.Application.Interfaces;
using BookVerse.Core.Entities;
using BookVerse.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookVerse.Infrastructure.Services;

public class BooksService : IBooksService
{
    private readonly IBookRepository _repository;
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;

    public BooksService(IBookRepository repository, IMapper mapper,AppDbContext context)
    {
        _repository = repository;
        _mapper = mapper;
        _context = context;
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
            await _repository.AddAsync(book);
            foreach (var authorId in bookDto.AuthorIds)
            {
                var bookAuthor = new BookAuthor
                {
                    BookId = book.Id,
                    AuthorId = authorId,
                    CreatedAtUtc = DateTime.Now
                };
                _context.BookAuthors.Add(bookAuthor);
            }

            foreach (var categoryId in bookDto.CategoryIds)
            {
                var bookCategory = new BookCategory
                {
                    BookId = book.Id,
                    CategoryId = categoryId,
                    CreatedAtUtc = DateTime.UtcNow
                };
                _context.BookCategories.Add(bookCategory);
            }
            await _context.SaveChangesAsync();

            var createdBook = await _repository.GetByIdAsync(book.Id);
            return _mapper.Map<BookReadDto>(createdBook!);
        }
        
    }

    public async Task<bool> UpdateAsync(int id, BookUpdateDto bookDto)
    {

        var book = await _repository.GetByIdAsync(id);
        if (book == null) return false;

        // Update basic properties
        _mapper.Map(bookDto, book);

        // Update author relationships
        var existingAuthorRelations = await _context.BookAuthors
            .Where(ba => ba.BookId == id)
            .ToListAsync();
        _context.BookAuthors.RemoveRange(existingAuthorRelations);

        foreach (var authorId in bookDto.AuthorIds)
        {
            var bookAuthor = new BookAuthor
            {
                BookId = id,
                AuthorId = authorId,
                CreatedAtUtc = DateTime.UtcNow
            };
            _context.BookAuthors.Add(bookAuthor);
        }

        // Update category relationships
        var existingCategoryRelations = await _context.BookCategories
            .Where(bc => bc.BookId == id)
            .ToListAsync();
        _context.BookCategories.RemoveRange(existingCategoryRelations);

        foreach (var categoryId in bookDto.CategoryIds)
        {
            var bookCategory = new BookCategory
            {
                BookId = id,
                CategoryId = categoryId,
                CreatedAtUtc = DateTime.UtcNow
            };
            _context.BookCategories.Add(bookCategory);
        }

        await _context.SaveChangesAsync();
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