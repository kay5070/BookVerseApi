namespace BookStoreApi.Dtos.User;

public record RegisterResponse: BasicResponse
{
    public IEnumerable<string?> Errors { get; init; }
}