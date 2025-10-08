using System.Linq.Expressions;
using BookStoreApi.Data;
using BookStoreApi.Entities;
using BookStoreApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Repositories;

public class BookRepository : GenericRepository<Book>,IBookRepository
{
    private readonly AppDbContext _context;
    
    public BookRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _dbSet.Include(b => b.Author).Include(b => b.Category).ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _dbSet.Include(b => b.Author).Include(b => b.Category)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Book>> FindAsync(Expression<Func<Book, bool>> predicate)
    {
        return await _dbSet.Include(b => b.Author).Include(b => b.Category).Where(predicate).ToListAsync();
    }

    public async Task<Book?> GetExistingBook(Book book)
    {
        return await _dbSet.Include(b => b.Author).Include(b=>b.Category).FirstOrDefaultAsync(b => b.Title == book.Title);
    }
}