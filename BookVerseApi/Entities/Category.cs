using System.ComponentModel.DataAnnotations;

namespace BookVerseApi.Entities;

public class Category
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Book> Books { get; set; } = new  List<Book>();
}