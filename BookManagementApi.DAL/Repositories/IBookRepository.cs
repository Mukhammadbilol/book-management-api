using BookManagementApi.Domain.Models;

namespace BookManagementApi.DAL.Repositories;

public interface IBookRepository
{
    Task AddBookAsync(Book book);
    Task AddBooksAsync(IEnumerable<Book> books);
    Task<IEnumerable<Book>> GetAllBooksAsync();
    Task<Book> GetBookByIdAsync(Guid id);
    Task<IEnumerable<Book>> GetPopularBooksAsync(int pageIndex, int pageSize);
    Task UpdateBookAsync(Book book);
    Task SoftDeleteBookAsync(Guid id);
    Task SoftDeleteBooksAsync(IEnumerable<Guid> ids);
}
