using BookManagementApi.Domain.DTOs;

namespace BookManagementApi.Services;

public interface IBookService
{
    Task<IEnumerable<GetBookDto>> GetAllBooksAsync();
    Task<BookDto> GetBookByIdAsync(Guid id);
    Task<IEnumerable<GetPopularBookDto>> GetPopularBooksAsync(int pageIndex, int pageSiz);
    Task AddBookAsync(CreateBookDto bookDto);
    Task AddBooksAsync(IEnumerable<CreateBookDto> bookDtos);
    Task UpdateBookAsync(Guid id, UpdateBookDto bookDto);
    Task SoftDeleteBookAsync(Guid id);
    Task SoftDeleteBooksAsync(IEnumerable<Guid> ids);
}
