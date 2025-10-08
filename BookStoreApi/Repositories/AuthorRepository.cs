using BookStoreApi.Data;
using BookStoreApi.Entities;
using BookStoreApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Repositories;

public class AuthorRepository:GenericRepository<Author>,IAuthorRepository
{
    private readonly AppDbContext _context;

    public AuthorRepository(AppDbContext context):base(context)
    {
        _context = context;
    }

    public async Task<Author?> GetByNameAsync(string firstName, string lastName)
    {
        return await _dbSet.Include(a=>a.Books).Where(a => a.FirstName == firstName && a.LastName == lastName).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Author>> GetAllAsync()
    {
        return await _dbSet.Include(a=>a.Books).ToListAsync();
    }

    public async Task<Author?> GetByIdAsync(int id)
    {
        return await _dbSet.Include(a => a.Books).FirstOrDefaultAsync(a => a.Id == id);
    }
}