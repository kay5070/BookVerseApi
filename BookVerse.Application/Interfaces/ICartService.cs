using BookVerse.Application.Dtos.Cart;
using BookVerse.Application.Dtos.User;

namespace BookVerse.Application.Interfaces;

public interface ICartService
{
    Task<CartDto?> GetUserCartAsync(Guid userId);
    Task<CartDto> AddToCartAsync(Guid userId, CartItemAdd cartItem);
    Task<CartDto?> UpdateCartItemAsync(Guid userId, int cartItemId, CartItemUpdate cartItemUpdate);
    Task<BasicResponse> RemoveCartItemAsync(Guid userId, int cartItemId);
    Task<BasicResponse> ClearCartAsync(Guid userId);
}