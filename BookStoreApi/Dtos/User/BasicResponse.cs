namespace BookStoreApi.Dtos.User;

public record BasicResponse
{
    public required bool Succeeded { get; init; }
    public string? Message  { get; init; }
}