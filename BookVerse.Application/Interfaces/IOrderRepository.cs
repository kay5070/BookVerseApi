using BookVerse.Core.Entities;
using BookVerse.Core.Models;

namespace BookVerse.Application.Interfaces;

public interface IOrderRepository : IGenericRepository<Order>
{
    Task<Order?> GetOrderWithDetailsAsync(int orderId);
    Task<PagedResult<Order>> GetUserOrdersAsync(Guid userId, QueryParameters parameters);
    Task<PagedResult<Order>> GetAllOrdersAsync(QueryParameters parameters);
    Task<Order?> GetUserOrderByIdAsync(Guid userId, int orderId);
    Task<bool> OrderExistsForUserAsync(Guid userId, int orderId);
}