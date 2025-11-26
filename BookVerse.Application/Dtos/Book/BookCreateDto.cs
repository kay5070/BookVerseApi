using System.ComponentModel.DataAnnotations;

namespace BookVerse.Application.Dtos.Book;

public class BookCreateDto
{   
    [Required(ErrorMessage = "Title is required")]
    [MaxLength(100, ErrorMessage = "Max length is 100")]
    public string Title { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Publish Data is required")]
    public DateOnly PublishDate { get; set; }

    [Range(1,1000,ErrorMessage = "Range is between 1 and 1000")]
    public decimal Price { get; set; }
    
    [Required(ErrorMessage = "At least one author is required")]
    [MinLength(1, ErrorMessage = "At least one author is required")]
    public List<int> AuthorIds { get; set; } = new();
    
    [Required(ErrorMessage = "At least one category is required")]
    [MinLength(1, ErrorMessage = "At least one category is required")]
    public List<int> CategoryIds { get; set; } = new();
}