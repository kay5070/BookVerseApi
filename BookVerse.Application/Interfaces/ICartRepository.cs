using BookVerse.Core.Entities;

namespace BookVerse.Application.Interfaces;

public interface ICartRepository : IGenericRepository<Cart>
{
    Task<Cart?> GetUserCartAsync(Guid userId);
    Task<Cart?> GetCartWithItemsAsync(int cartId);
    Task<CartItem?> GetCartItemAsync(int cartId, int bookId);
    Task AddCartItemAsync(CartItem cartItem);
    void UpdateCartItem (CartItem cartItem);
    void DeleteCartItem(CartItem cartItem);
    Task ClearCartAsync(int cartId);
}