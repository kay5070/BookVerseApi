using BookVerseApi.Entities;

namespace BookVerseApi.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
}