using BookManagementApi.Domain.DTOs;
using BookManagementApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController(IBookService bookService) : ControllerBase
{
    private readonly IBookService _bookService = bookService;

    [HttpGet]
    public async Task<IActionResult> GetAllBooks()
    {
        var books = await _bookService.GetAllBooksAsync();
        return Ok(books);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBookById(Guid id)
    {
        var book = await _bookService.GetBookByIdAsync(id);

        return book == null ? NotFound("Book not found") : Ok(book);
    }

    [HttpGet("popular")]
    public async Task<IActionResult> GetPopularBooks([FromQuery] int pageIndex = 1, int pageSize = 10)
    {
        var books = await _bookService.GetPopularBooksAsync(pageIndex, pageSize);
        return Ok(books);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AddBook([FromBody] CreateBookDto book)
    {
        if (!ModelState.IsValid)
                return BadRequest(ModelState);
        if (book == null)
            return BadRequest("Invalid book data");

        await _bookService.AddBookAsync(book);
        return CreatedAtAction(nameof(GetAllBooks), new { }, "Book added successfully");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("bulk")]
    public async Task<IActionResult> AddBooks([FromBody] IEnumerable<CreateBookDto> books)
    {
        if (!ModelState.IsValid)
                return BadRequest(ModelState);
        if (books == null || !books.Any())
        {
            return BadRequest("No books provided");
        }

        await _bookService.AddBooksAsync(books);
        return Ok("Books added successfully");
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateBook(Guid id, [FromBody] UpdateBookDto book)
    {
        if (!ModelState.IsValid)
                return BadRequest(ModelState);
        if (book == null)
            return BadRequest("Invalid book data");

        await _bookService.UpdateBookAsync(id, book);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id::guid}")]
    public async Task<IActionResult> SoftDeleteBook(Guid id)
    {
        await _bookService.SoftDeleteBookAsync(id);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("bulk")]
    public async Task<IActionResult> SoftDeleteBooks([FromBody] IEnumerable<Guid> ids)
    {
        if (ids == null || !ids.Any())
        {
            return BadRequest("No book IDs provided");
        }

        await _bookService.SoftDeleteBooksAsync(ids);
        return NoContent();
    }
}
