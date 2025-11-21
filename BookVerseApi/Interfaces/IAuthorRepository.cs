using BookVerseApi.Entities;

namespace BookVerseApi.Interfaces;

public interface IAuthorRepository : IGenericRepository<Author>
{
    Task<Author?> GetByNameAsync(string firstName,string lastName);
    
    Task<IEnumerable<Author>> GetAllAsync();
    Task<Author?> GetByIdAsync(int id);

}