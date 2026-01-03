using BookVerse.Application.Interfaces;
using BookVerse.Core.Entities;
using BookVerse.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookVerse.Infrastructure.Repositories;

public class CartRepository : GenericRepository<Cart>,ICartRepository
{
    private readonly AppDbContext _context;

    public CartRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Cart?> GetUserCartAsync(Guid userId)
    {
          return await _dbSet.Include(c => c.CartItems).ThenInclude(ci => ci.Book).FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task<Cart?> GetCartWithItemsAsync(int cartId)
    {
        return await _dbSet.Include(c => c.CartItems).ThenInclude(ci => ci.Book)
            .FirstOrDefaultAsync(c => c.Id == cartId);
    }

    public async Task<CartItem?> GetCartItemAsync(int cartId, int bookId)
    {
        return await _context.CartItems.FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.BookId == bookId);
    }

    public async Task AddCartItemAsync(CartItem cartItem)
    {
         await _context.CartItems.AddAsync(cartItem);
    }

    public void UpdateCartItem(CartItem cartItem)
    {
        _context.CartItems.Update(cartItem);
    }

    public void DeleteCartItem(CartItem cartItem)
    {
        _context.CartItems.Remove(cartItem);
    }

    public async Task ClearCartAsync(int cartId)
    {
        var cartItems = await _context.CartItems.Where(ci => ci.CartId == cartId).ToListAsync();
        _context.CartItems.RemoveRange(cartItems);
    }
}