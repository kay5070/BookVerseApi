using BookVerse.Core.Models;
namespace BookVerse.Application.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<PagedResult<T>> GetPagedAsync(QueryParameters parameters);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
}