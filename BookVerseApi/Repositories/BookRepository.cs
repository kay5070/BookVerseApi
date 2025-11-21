using System.Linq.Expressions;
using BookVerseApi.Data;
using BookVerseApi.Entities;
using BookVerseApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookVerseApi.Repositories;

public class BookRepository : GenericRepository<Book>,IBookRepository
{
    private readonly AppDbContext _context;
    
    public BookRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<IEnumerable<Book>> GetAllAsync()
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