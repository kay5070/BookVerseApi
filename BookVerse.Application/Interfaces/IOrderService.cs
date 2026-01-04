using BookVerse.Application.Dtos.Order;
using BookVerse.Application.Dtos.User;
using BookVerse.Core.Models;

namespace BookVerse.Application.Interfaces;

public interface IOrderService
{
    Task<OrderReadDto> CreateOrderFromCartAsync(Guid userId, OrderCreateDto orderCreateDto);
    Task<PagedResult<OrderListDto>> GetUserOrdersAsync(Guid userId, QueryParameters parameters);
    Task<PagedResult<OrderListDto>> GetAllOrdersAsync(QueryParameters parameters);
    Task<OrderReadDto?> GetOrderByIdAsync(Guid userId, int orderId, bool isAdmin = false);
    Task<BasicResponse> CancelOrderAsync(Guid userId, int orderId);
    Task<BasicResponse> UpdateOrderStatusAsync(int orderId, OrderUpdateStatusDto updateDto);
    Task<BasicResponse> UpdatePaymentStatusAsync(int orderId, PaymentUpdateStatusDto updateDto);
}