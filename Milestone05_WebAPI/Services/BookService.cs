using Milestone05_WebAPI.DTOs;
using Milestone05_WebAPI.Models;
using Milestone05_WebAPI.Repositories;

namespace Milestone05_WebAPI.Services;

public class BookService
{
    private readonly BookRepository _bookRepository;
    private readonly GenreRepository _genreRepository;
    private readonly UserRepository _userRepository;

    public BookService(BookRepository bookRepository, GenreRepository genreRepository, UserRepository userRepository)
    {
        _bookRepository = bookRepository;
        _genreRepository = genreRepository;
        _userRepository = userRepository;
    }

    public List<BookDto> GetBooks(int? userId, string? search)
    {
        string? pattern = string.IsNullOrWhiteSpace(search) ? null : "%" + search.Trim() + "%";

        var result = new List<BookDto>();
        foreach (var book in _bookRepository.GetAll(userId, pattern))
        {
            result.Add(ToDto(book));
        }
        return result;
    }

    public ServiceResult<BookDto> GetBook(int id)
    {
        var book = _bookRepository.GetById(id);
        if (book == null)
        {
            return ServiceResult<BookDto>.Fail(ErrorType.NotFound, "Book not found.");
        }
        return ServiceResult<BookDto>.Ok(ToDto(book));
    }

    public ServiceResult<BookDto> CreateBook(CreateBookDto dto)
    {
        string title = dto.Title.Trim();
        string author = dto.Author.Trim();

        string? error = ValidateFields(title, author, dto.Year, dto.GenreId);
        if (error != null)
        {
            return ServiceResult<BookDto>.Fail(ErrorType.Validation, error);
        }
        if (_userRepository.FindById(dto.UserId) == null)
        {
            return ServiceResult<BookDto>.Fail(ErrorType.Validation, "The user does not exist.");
        }
        if (_bookRepository.TitleExists(dto.UserId, title, null))
        {
            return ServiceResult<BookDto>.Fail(ErrorType.Conflict, "This user already has a book with that title.");
        }

        var book = new Book
        {
            UserId = dto.UserId,
            GenreId = dto.GenreId,
            Title = title,
            Author = author,
            Year = dto.Year,
            Status = dto.Status
        };
        int id = _bookRepository.Add(book);

        return GetBook(id);
    }

    public ServiceResult<BookDto> UpdateBook(int id, UpdateBookDto dto)
    {
        var existing = _bookRepository.GetById(id);
        if (existing == null)
        {
            return ServiceResult<BookDto>.Fail(ErrorType.NotFound, "Book not found.");
        }

        string title = dto.Title.Trim();
        string author = dto.Author.Trim();

        string? error = ValidateFields(title, author, dto.Year, dto.GenreId);
        if (error != null)
        {
            return ServiceResult<BookDto>.Fail(ErrorType.Validation, error);
        }
        if (_bookRepository.TitleExists(existing.UserId, title, id))
        {
            return ServiceResult<BookDto>.Fail(ErrorType.Conflict, "This user already has a book with that title.");
        }

        existing.GenreId = dto.GenreId;
        existing.Title = title;
        existing.Author = author;
        existing.Year = dto.Year;
        existing.Status = dto.Status;
        _bookRepository.Update(existing);

        return GetBook(id);
    }

    public ServiceResult<bool> DeleteBook(int id)
    {
        if (!_bookRepository.Delete(id))
        {
            return ServiceResult<bool>.Fail(ErrorType.NotFound, "Book not found.");
        }
        return ServiceResult<bool>.Ok(true);
    }

    private string? ValidateFields(string title, string author, int year, int genreId)
    {
        if (title == "")
        {
            return "Title cannot be empty.";
        }
        if (author == "")
        {
            return "Author cannot be empty.";
        }
        if (year < 1 || year > DateTime.Now.Year)
        {
            return $"Year must be between 1 and {DateTime.Now.Year}.";
        }
        if (_genreRepository.GetById(genreId) == null)
        {
            return "That genre does not exist.";
        }
        return null;
    }

    private static BookDto ToDto(Book book)
    {
        return new BookDto
        {
            Id = book.Id,
            UserId = book.UserId,
            GenreId = book.GenreId,
            GenreName = book.GenreName,
            Title = book.Title,
            Author = book.Author,
            Year = book.Year,
            Status = book.Status
        };
    }
}