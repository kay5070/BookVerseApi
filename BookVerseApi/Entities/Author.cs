using System.ComponentModel.DataAnnotations;

namespace BookVerseApi.Entities;

public class Author
{
    [Key]
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
    // Navigation
    public ICollection<Book> Books { get; set; } = new List<Book>();

}