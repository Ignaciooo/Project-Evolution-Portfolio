using Milestone03_LayeredConsole.Models;
using Milestone03_LayeredConsole.Repositories;

namespace Milestone03_LayeredConsole.Services;

public class BookService
{
    private readonly BookRepository _bookRepository;
    private readonly AuthService _authService;

    public BookService(BookRepository bookRepository, AuthService authService)
    {
        _bookRepository = bookRepository;
        _authService = authService;
    }

    public (bool Success, string Message) AddBook(string title, string author, int year, ReadingStatus status)
    {
        var user = _authService.CurrentUser;
        if (user == null)
        {
            return (false, "You must log in first.");
        }

        title = title.Trim();
        author = author.Trim();

        if (title == "")
        {
            return (false, "Title cannot be empty.");
        }
        if (author == "")
        {
            return (false, "Author cannot be empty.");
        }
        if (year < 1 || year > DateTime.Now.Year)
        {
            return (false, $"Year must be between 1 and {DateTime.Now.Year}.");
        }
        if (TitleExists(user.Id, title))
        {
            return (false, "You already have a book with that title.");
        }

        var book = new Book
        {
            OwnerId = user.Id,
            Title = title,
            Author = author,
            Year = year,
            Status = status
        };
        _bookRepository.Add(book);

        return (true, "Book added successfully.");
    }

    public List<Book> GetMyBooks()
    {
        var user = _authService.CurrentUser;
        if (user == null)
        {
            return new List<Book>();
        }
        return _bookRepository.GetByOwner(user.Id);
    }

    public List<Book> FindMyBooks(string text)
    {
        var result = new List<Book>();
        foreach (var book in GetMyBooks())
        {
            if (book.Title.Contains(text, StringComparison.OrdinalIgnoreCase) ||
                book.Author.Contains(text, StringComparison.OrdinalIgnoreCase))
            {
                result.Add(book);
            }
        }
        return result;
    }

    public (bool Success, string Message) DeleteBook(int id)
    {
        var user = _authService.CurrentUser;
        if (user == null)
        {
            return (false, "You must log in first.");
        }

        var book = _bookRepository.GetById(id);
        if (book == null || book.OwnerId != user.Id)
        {
            return (false, "No book with that Id.");
        }

        _bookRepository.Remove(book);
        return (true, "Book deleted.");
    }

    private bool TitleExists(int ownerId, string title)
    {
        foreach (var book in _bookRepository.GetByOwner(ownerId))
        {
            if (string.Equals(book.Title, title, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }
}