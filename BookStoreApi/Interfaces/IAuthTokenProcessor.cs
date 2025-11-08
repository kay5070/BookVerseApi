namespace BookStoreApi.Dtos.User;

public interface IAuthTokenProcessor
{
    public (string jwtToken, DateTime expiresAtUtc) GenerateJwtToken(Entities.User user);
    public string GenerateRefreshToken();
}