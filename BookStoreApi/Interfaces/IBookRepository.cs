using System.Linq.Expressions;
using BookStoreApi.Entities;

namespace BookStoreApi.Interfaces;

public interface IBookRepository : IGenericRepository<Book>
{
    Task<IEnumerable<Book>> FindAsync(Expression<Func<Book, bool>> predicate);
    Task<Book?> GetByIdAsync(int id);
    Task<IEnumerable<Book>> GetAllAsync();
    Task<Book?> GetExistingBook(Book book);
}