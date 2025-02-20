namespace BookManagementApi.Domain.DTOs;

public class CreateBookDto
{
    public string Title { get; set; }
    public int PublicationYear { get; set; }
    public string AuthorName { get; set; }
}
