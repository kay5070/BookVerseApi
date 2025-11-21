using System.ComponentModel.DataAnnotations;

namespace BookVerseApi.Dtos.Book;

public class BookCreateDto
{   
    [Required(ErrorMessage = "Title is required")]
    [MaxLength(100, ErrorMessage = "Max length is 100")]
    public string Title { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Publish Data is required")]
    public DateOnly PublishDate { get; set; }

    [Range(1,1000,ErrorMessage = "Range is between 1 and 1000")]
    public decimal Price { get; set; }
    
    [Required(ErrorMessage = "AuthorId is required")]
    public int AuthorId { get; set; }
    
    [Required(ErrorMessage = "CategoryId is required")]
    public int CategoryId { get; set; }
}