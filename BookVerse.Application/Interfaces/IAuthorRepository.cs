using BookVerse.Core.Entities;

namespace BookVerse.Application.Interfaces;

public interface IAuthorRepository : IGenericRepository<Author>
{
    Task<Author?> GetByNameAsync(string firstName,string lastName);
    
    Task<IEnumerable<Author>> GetAllAsync();
    Task<Author?> GetByIdAsync(int id);

}