using System.Linq.Expressions;
using BookVerse.Application.Interfaces;
using BookVerse.Core.Entities;
using BookVerse.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookVerse.Infrastructure.Repositories;

public class BookRepository : GenericRepository<Book>,IBookRepository
{
    private readonly AppDbContext _context;
    
    public BookRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<IEnumerable<Book>> GetAllAsync()
    {
            var books = await _dbSet
            .Include(b => b.BookAuthors)
            .ThenInclude(ba => ba.Author)
            .Include(b => b.BookCategories)
            .ThenInclude(bc => bc.Category)
            .ToListAsync();
            return books;
    }
    

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _dbSet.Include(b => b.BookAuthors).ThenInclude(ba => ba.Author).Include(b => b.BookCategories)
            .ThenInclude(bc => bc.Category).FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Book>> FindAsync(Expression<Func<Book, bool>> predicate)
    {
        return await _dbSet
            .Include(b => b.BookAuthors)
            .ThenInclude(ba => ba.Author)
            .Include(b => b.BookCategories)
            .ThenInclude(bc => bc.Category)
            .Where(predicate)
            .ToListAsync();
    }

    public async Task<Book?> GetExistingBook(Book book)
    {
        return await _dbSet
            .Include(b => b.BookAuthors)
            .ThenInclude(ba => ba.Author)
            .Include(b => b.BookCategories)
            .ThenInclude(bc => bc.Category)
            .FirstOrDefaultAsync(b => b.Title == book.Title);
    }
}