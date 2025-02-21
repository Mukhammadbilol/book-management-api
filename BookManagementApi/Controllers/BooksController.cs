using BookManagementApi.Domain.DTOs;
using BookManagementApi.Services;
using BookManagementApi.Validators;
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

        return book == null ? NotFound("Book not found with the provided ID") : Ok(book);
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

        try
        {
            await _bookService.AddBookAsync(book);
            return CreatedAtAction(nameof(GetAllBooks), new { }, "Book added successfully");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("bulk")]
    public async Task<IActionResult> AddBooks([FromBody] IEnumerable<CreateBookDto> books)
    {
        if (books == null || !books.Any())
        {
            return BadRequest("No books provided");
        }

        var validator = new CreateBookDtoValidator();
        var errors = books.SelectMany(book => validator.Validate(book).Errors)
        .Where(error => error != null)
        .ToList();

        if (errors.Any())
        {
            return BadRequest(errors.Select(x => x.ErrorMessage));
        }

        try
        {
            await _bookService.AddBooksAsync(books);
            return CreatedAtAction(nameof(GetAllBooks), new { }, "Books added successfully");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateBook(Guid id, [FromBody] UpdateBookDto book)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (book == null)
            return BadRequest("Invalid book data");

        try
        {
            await _bookService.UpdateBookAsync(id, book);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id::guid}")]
    public async Task<IActionResult> SoftDeleteBook(Guid id)
    {
        try
        {
            await _bookService.SoftDeleteBookAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("bulk")]
    public async Task<IActionResult> SoftDeleteBooks([FromBody] IEnumerable<Guid> ids)
    {
        if (ids == null || !ids.Any())
        {
            return BadRequest("No book IDs provided");
        }

        try
        {
            await _bookService.SoftDeleteBooksAsync(ids);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
