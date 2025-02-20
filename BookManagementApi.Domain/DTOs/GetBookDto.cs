namespace BookManagementApi.Domain.DTOs;

public class GetBookDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public int PublicationYear { get; set; }
    public string AuthorName { get; set; }
    public int ViewsCount { get; set; }
}

