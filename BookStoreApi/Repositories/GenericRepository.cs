using System.Linq.Expressions;
using BookStoreApi.Data;
using BookStoreApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Repositories;

public class GenericRepository<T>:IGenericRepository<T> where T : class
{
    private readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;
    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }
    // public virtual async Task<IEnumerable<T>> GetAllAsync()
    // {
    //     return await _dbSet.ToListAsync();
    // }
    //
    // public virtual async Task<T?> GetByIdAsync(int id)
    // {
    //     return await _dbSet.FindAsync(id);
    // }
    //
    // public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    // {
    //     return await _dbSet.Where(predicate).ToListAsync();
    // }

    public virtual async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await SaveAsync();
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}