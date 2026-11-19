using Microsoft.AspNetCore.Mvc;
using Milestone05_WebAPI.DTOs;
using Milestone05_WebAPI.Services;

namespace Milestone05_WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ApiControllerBase
{
    private readonly BookService _bookService;

    public BooksController(BookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<BookDto>), StatusCodes.Status200OK)]
    public ActionResult<List<BookDto>> GetBooks([FromQuery] int? userId, [FromQuery] string? search)
    {
        return Ok(_bookService.GetBooks(userId, search));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BookDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public ActionResult<BookDto> GetBook(int id)
    {
        var result = _bookService.GetBook(id);
        if (!result.Success)
        {
            return ErrorResponse(result);
        }
        return Ok(result.Value);
    }

    [HttpPost]
    [ProducesResponseType(typeof(BookDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public ActionResult<BookDto> CreateBook(CreateBookDto dto)
    {
        var result = _bookService.CreateBook(dto);
        if (!result.Success)
        {
            return ErrorResponse(result);
        }
        return CreatedAtAction(nameof(GetBook), new { id = result.Value!.Id }, result.Value);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(BookDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public ActionResult<BookDto> UpdateBook(int id, UpdateBookDto dto)
    {
        var result = _bookService.UpdateBook(id, dto);
        if (!result.Success)
        {
            return ErrorResponse(result);
        }
        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public IActionResult DeleteBook(int id)
    {
        var result = _bookService.DeleteBook(id);
        if (!result.Success)
        {
            return ErrorResponse(result);
        }
        return NoContent();
    }
}
