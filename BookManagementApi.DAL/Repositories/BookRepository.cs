using Azure;
using BookManagementApi.DAL.Data;
using BookManagementApi.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManagementApi.DAL.Repositories;

public class BookRepository(AppDbContext context) : IBookRepository
{
    private readonly AppDbContext _context = context;

    public async Task AddBookAsync(Book book)
    {
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
    }

    public async Task AddBooksAsync(IEnumerable<Book> books)
    {
        await _context.Books.AddRangeAsync(books);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Book>> GetAllBooksAsync()
        => await _context.Books
            .Where(b => !b.IsDeleted)
            .AsNoTracking()
            .ToListAsync();

    public async Task<Book> GetBookByIdAsync(Guid id)
        => await _context.Books
            .Where(b => b.Id == id)
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<Book>> GetPopularBooksAsync(int pageIndex, int pageSize)
        => await _context.Books
            .Where(b => !b.IsDeleted)
            .OrderByDescending(b => b.ViewsCount)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();

    public async Task UpdateBookAsync(Book book)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync();
    }

    public async Task SoftDeleteBookAsync(Guid id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book != null && !book.IsDeleted)
        {
            book.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task SoftDeleteBooksAsync(IEnumerable<Guid> ids)
    {
        var books = await _context.Books
            .Where(b => ids.Contains(b.Id) && !b.IsDeleted)
            .ToListAsync();

        if (books.Any())
        {
            books.ForEach(book => book.IsDeleted = true);
            await _context.SaveChangesAsync();
        }
    }
}
