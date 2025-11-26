using BookVerse.Core.Entities;

namespace BookVerse.Application.Interfaces;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    Task<Category?> GetByNameAsync(string name);
   
}