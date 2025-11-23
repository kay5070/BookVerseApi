using BookVerseApi.Dtos.Book;

namespace BookVerseApi.Dtos.Author;

public class AuthorReadDto
{
    public int  Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public ICollection<BookBriefDto> Books { get; set; } = new List<BookBriefDto>();
}