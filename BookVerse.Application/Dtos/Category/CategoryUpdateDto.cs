using System.ComponentModel.DataAnnotations;

namespace BookVerse.Application.Dtos.Category;

public class CategoryUpdateDto
{
    [Required(ErrorMessage = "Category Name is required")]
    [MaxLength(100, ErrorMessage = "Max length is 100")]
    public string Name { get; set; } = string.Empty;
}