using BookVerse.Application.Interfaces;
using BookVerse.Core.Entities;
using BookVerse.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookVerse.Infrastructure.Repositories;

public class AuthorRepository:GenericRepository<Author>,IAuthorRepository
{
    private readonly AppDbContext _context;

    public AuthorRepository(AppDbContext context):base(context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Author>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }
    public async Task<Author?> GetByNameAsync(string firstName, string lastName)
    {
        return await _dbSet
            .Include(a => a.BookAuthors)
            .ThenInclude(ba => ba.Book)
            .FirstOrDefaultAsync(a => a.FirstName == firstName && a.LastName == lastName);
    }
    
    public async Task<Author?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(a => a.BookAuthors)
            .ThenInclude(ba => ba.Book)
            .FirstOrDefaultAsync(a => a.Id == id);
    }
}