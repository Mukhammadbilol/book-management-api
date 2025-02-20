namespace BookManagementApi.Domain.DTOs;

public class UpdateBookDto
{
    public string Title { get; set; }
    public int PublicationYear { get; set; }
    public string AuthorName { get; set; }
    public bool IsDeleted { get; set; }
}
