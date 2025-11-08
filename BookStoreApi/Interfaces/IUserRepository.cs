using BookStoreApi.Entities;

namespace BookStoreApi.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
}