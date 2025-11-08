namespace BookStoreApi.Dtos.User;

public record RefreshTokenRequest
{
    public string? RefreshToken { get; init; }
}