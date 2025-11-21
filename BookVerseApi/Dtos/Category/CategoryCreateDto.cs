using System.ComponentModel.DataAnnotations;

namespace BookVerseApi.Dtos.Category;

public class CategoryCreateDto
{
    [Required(ErrorMessage = "Category Name is required")]
    [MaxLength(100, ErrorMessage = "Max length is 100")]
    public string Name { get; set; } = string.Empty;
}