namespace BookVerseApi.Dtos.Book;

public class BookReadDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    
    public int AuthorId { get; set; }
    public string Author { get; set; } = string.Empty;
    
    public int CategoryId { get; set; }
    
    public string Category { get; set; } = string.Empty;

    public DateOnly PublishDate { get; set; }
    public decimal Price { get; set; }
    
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}