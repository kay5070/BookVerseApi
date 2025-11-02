namespace BookStoreApi.Dtos.User;

public record RegisterResponse
{
    public bool Succeeded { get; init; }
    public string? Message { get; init; }
    public IEnumerable<string?> Errors { get; init; }
}