namespace BookStoreApi.Dtos.User;

public record UserProfileDto
{
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
}