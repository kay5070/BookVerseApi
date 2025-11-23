namespace BookVerseApi.Dtos.User;

// public record UserProfileDto
// {
//     public required string Email { get; init; }
//     public required string FirstName { get; init; }
//     public required string LastName { get; init; }
//     public DateTime CreatedAtUtc { get; set; }
//     public DateTime? UpdatedAtUtc { get; set; }
// }

public class UserProfileDto
{
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
}