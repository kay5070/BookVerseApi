using Microsoft.AspNetCore.Identity;

namespace BookVerse.Core.Entities;

public class User : IdentityUser<Guid>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string?  RefreshToken { get; set; }
    public DateTime?  RefreshTokenExpiresAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    public static User Create(string email, string firstName, string lastName)
    {
        return new User
        {
            Email = email,
            UserName = email,
            FirstName = firstName,
            LastName = lastName,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };
    }

    public override string ToString()
    {
        return FirstName + " " + LastName;
    }
}