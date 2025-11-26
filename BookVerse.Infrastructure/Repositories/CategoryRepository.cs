using BookVerse.Application.Interfaces;
using BookVerse.Core.Entities;
using BookVerse.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookVerse.Infrastructure.Repositories;

public class CategoryRepository:GenericRepository<Category>,ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context):base(context)
    {
        _context = context;
    }
    
    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(c => c.BookCategories)
            .ThenInclude(bc => bc.Book)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Category?> GetByNameAsync(string name)
    {
        return await _dbSet
            .Include(c => c.BookCategories)
            .ThenInclude(bc => bc.Book)
            .FirstOrDefaultAsync(c => c.Name == name);
    }
}