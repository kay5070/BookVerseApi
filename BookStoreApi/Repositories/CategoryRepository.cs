using BookStoreApi.Data;
using BookStoreApi.Entities;
using BookStoreApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Repositories;

public class CategoryRepository:GenericRepository<Category>,ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context):base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _dbSet.Include(c => c.Books).FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Category?> GetByNameAsync(string name)
    {
        return await _dbSet.Include(c => c.Books).FirstOrDefaultAsync(c => c.Name == name);
    }
}