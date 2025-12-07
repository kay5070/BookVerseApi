using System.ComponentModel.DataAnnotations;

namespace BookVerse.Core.Entities;

public class Category
{
    [Key] public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();
}