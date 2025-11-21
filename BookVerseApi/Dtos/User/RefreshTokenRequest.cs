using System.ComponentModel.DataAnnotations;

namespace BookVerseApi.Dtos.User;

public record RefreshTokenRequest
{
    [Required(ErrorMessage = "Refresh token is required")]
    public string? RefreshToken { get; init; }
}