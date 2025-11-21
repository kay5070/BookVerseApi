using BookVerseApi.Data;
using BookVerseApi.Entities;
using BookVerseApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookVerseApi.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        return user;
    }
}