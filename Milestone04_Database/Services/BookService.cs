using Milestone04_Database.Models;
using Milestone04_Database.Repositories;

namespace Milestone04_Database.Services;

public class BookService
{
    private readonly BookRepository _bookRepository;
    private readonly GenreRepository _genreRepository;
    private readonly AuthService _authService;

    public BookService(BookRepository bookRepository, GenreRepository genreRepository, AuthService authService)
    {
        _bookRepository = bookRepository;
        _genreRepository = genreRepository;
        _authService = authService;
    }

    public List<Genre> GetGenres()
    {
        return _genreRepository.GetAll();
    }

    public (bool Success, string Message) AddBook(string title, string author, int year, int genreId, ReadingStatus status)
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
        if (_genreRepository.GetById(genreId) == null)
        {
            return (false, "That genre does not exist.");
        }
        if (_bookRepository.TitleExists(user.Id, title))
        {
            return (false, "You already have a book with that title.");
        }

        var book = new Book
        {
            UserId = user.Id,
            GenreId = genreId,
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
        return _bookRepository.GetByUser(user.Id);
    }

    public List<Book> FindMyBooks(string text)
    {
        var user = _authService.CurrentUser;
        if (user == null)
        {
            return new List<Book>();
        }
        return _bookRepository.Search(user.Id, text);
    }

    public (bool Success, string Message) DeleteBook(int id)
    {
        var user = _authService.CurrentUser;
        if (user == null)
        {
            return (false, "You must log in first.");
        }

        if (!_bookRepository.Delete(id, user.Id))
        {
            return (false, "No book with that Id.");
        }
        return (true, "Book deleted.");
    }

    public List<GenreCount> GetStatistics()
    {
        var user = _authService.CurrentUser;
        if (user == null)
        {
            return new List<GenreCount>();
        }
        return _bookRepository.CountByGenre(user.Id);
    }
}
