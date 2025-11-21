using BookVerseApi.Dtos.Book;

namespace BookVerseApi.Dtos.Category;

public class CategoryReadDto
{
    public int  Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public ICollection<BookBriefDto> Books { get; set; } = new List<BookBriefDto>();
}