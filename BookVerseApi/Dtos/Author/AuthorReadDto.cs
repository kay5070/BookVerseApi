using BookVerseApi.Dtos.Book;

namespace BookVerseApi.Dtos.Author;

public class AuthorReadDto
{
    public int  Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    
    public ICollection<BookBriefDto> Books { get; set; } = new List<BookBriefDto>();
}