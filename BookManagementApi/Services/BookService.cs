using BookManagementApi.DAL.Repositories;
using BookManagementApi.Domain.DTOs;
using BookManagementApi.Domain.Models;

namespace BookManagementApi.Services;

public class BookService(IBookRepository repository) : IBookService
{
    private readonly IBookRepository _repository = repository;

    public async Task AddBookAsync(CreateBookDto bookDto)
    {
        var book = new Book
        {
            Title = bookDto.Title,
            PublicationYear = bookDto.PublicationYear,
            AuthorName = bookDto.AuthorName
        };

        await _repository.AddBookAsync(book);
    }

    public async Task AddBooksAsync(IEnumerable<CreateBookDto> bookDtos)
    {
        var books = bookDtos.Select(dto => new Book
        {
            Title = dto.Title,
            PublicationYear = dto.PublicationYear,
            AuthorName = dto.AuthorName
        }).ToList();

        await _repository.AddBooksAsync(books);
    }

    public async Task<IEnumerable<GetBookDto>> GetAllBooksAsync()
    {
        var books = await _repository.GetAllBooksAsync();

        return books.Select(book => new GetBookDto
        {
            Id = book.Id,
            Title = book.Title,
            PublicationYear = book.PublicationYear,
            AuthorName = book.AuthorName,
            ViewsCount = book.ViewsCount
        }).ToList();
    }

    public async Task<BookDto> GetBookByIdAsync(Guid id)
    {
        var book = await _repository.GetBookByIdAsync(id);

        if (book == null)
            return null;

        book.ViewsCount++;
        await _repository.UpdateBookAsync(book);

        int yearsSincePublished = DateTime.Now.Year - book.PublicationYear;
        double popularityScore = (book.ViewsCount * 0.5) + (yearsSincePublished * 2);
        
        return new BookDto
        {
            Title = book.Title,
            PublicationYear = book.PublicationYear,
            AuthorName = book.AuthorName,
            ViewsCount = book.ViewsCount,
            PopularityScore = popularityScore
        };
    }

    public async Task<IEnumerable<GetPopularBookDto>> GetPopularBooksAsync(int pageIndex, int pageSize)
    {
        var books = await _repository.GetPopularBooksAsync(pageIndex, pageSize);

        return books.Select(book => new GetPopularBookDto
        {
            Title = book.Title
        }).ToList();
    }

    public async Task UpdateBookAsync(Guid id, UpdateBookDto bookDto)
    {
        var existingBook = await _repository.GetBookByIdAsync(id);

        if (existingBook == null)
        {
            throw new InvalidOperationException("Book not found with the provided ID");
        }

        existingBook.Title = bookDto.Title;
        existingBook.PublicationYear = bookDto.PublicationYear;
        existingBook.AuthorName = bookDto.AuthorName;
        existingBook.IsDeleted = bookDto.IsDeleted;

        await _repository.UpdateBookAsync(existingBook);
    }

    public async Task SoftDeleteBookAsync(Guid id)
    {
        var book = await _repository.GetBookByIdAsync(id);
        if (book == null)
        {
            throw new InvalidOperationException("Book not found with the provided ID");
        }
        else if (book.IsDeleted)
        {
            throw new InvalidOperationException("Book already deleted");
        }
    
        await _repository.SoftDeleteBookAsync(id);
    }

    public async Task SoftDeleteBooksAsync(IEnumerable<Guid> ids)
    {
        var existingBooks = await _repository.GetAllBooksAsync();
        var books = existingBooks.Where(b => ids.Contains(b.Id) && !b.IsDeleted);

        if (!books.Any())
        {
            throw new InvalidOperationException("No books found with the provided IDs or all are already deleted");
        }

        await _repository.SoftDeleteBooksAsync(ids);
    }
}
