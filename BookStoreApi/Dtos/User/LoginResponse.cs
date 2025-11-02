namespace BookStoreApi.Dtos.User;

public record LoginResponse
{
    public required bool Succeeded { get; init; }
    public string? Message { get; init; }
    public string? AccessToken { get; init; }
    public DateTime? ExpiresAtUtc{ get; init; }
    public string? RefreshToken{ get; init; }
    
}