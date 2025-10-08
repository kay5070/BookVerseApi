namespace BookStoreApi.Dtos.Author;

public class AuthorsReadDto
{
    public int  Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}