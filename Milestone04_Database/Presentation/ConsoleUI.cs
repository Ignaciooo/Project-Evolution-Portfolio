using Milestone04_Database.Models;
using Milestone04_Database.Services;

namespace Milestone04_Database.Presentation;

public class ConsoleUI
{
    private readonly AuthService _authService;
    private readonly BookService _bookService;
    private bool _running = true;

    public ConsoleUI(AuthService authService, BookService bookService)
    {
        _authService = authService;
        _bookService = bookService;
    }

    public void Run()
    {
        while (_running)
        {
            if (_authService.IsLoggedIn)
            {
                ShowUserMenu();
            }
            else
            {
                ShowGuestMenu();
            }
        }

        Console.WriteLine("Goodbye!");
    }

    private void ShowHeader()
    {
        Console.WriteLine();
        Console.WriteLine(" ━━━━━━━━━━━━━━━━━━━━━");
        Console.WriteLine("    PERSONAL LIBRARY");
        Console.WriteLine(" ━━━━━━━━━━━━━━━━━━━━━");
    }

    private void ShowGuestMenu()
    {
        ShowHeader();
        Console.WriteLine("1. Register");
        Console.WriteLine("2. Login");
        Console.WriteLine("0. Exit\n");
        Console.Write("Choose an option: ");

        switch (Console.ReadLine() ?? "")
        {
            case "1":
                Register();
                break;
            case "2":
                Login();
                break;
            case "0":
                _running = false;
                break;
            default:
                Console.WriteLine("Invalid option. Try again.");
                break;
        }
    }

    private void ShowUserMenu()
    {
        ShowHeader();
        Console.WriteLine($" Logged in as: {_authService.CurrentUser?.Username}\n");
        Console.WriteLine("1. Add book");
        Console.WriteLine("2. List books");
        Console.WriteLine("3. Find book");
        Console.WriteLine("4. Delete book");
        Console.WriteLine("5. Statistics");
        Console.WriteLine("6. Logout");
        Console.WriteLine("0. Exit\n");
        Console.Write("Choose an option: ");

        switch (Console.ReadLine() ?? "")
        {
            case "1":
                AddBook();
                break;
            case "2":
                ListBooks();
                break;
            case "3":
                FindBooks();
                break;
            case "4":
                DeleteBook();
                break;
            case "5":
                ShowStatistics();
                break;
            case "6":
                Logout();
                break;
            case "0":
                _running = false;
                break;
            default:
                Console.WriteLine("Invalid option. Try again.");
                break;
        }
    }

    private void Register()
    {
        PrintTitle("Register");
        string username = ReadNonEmpty("Username: ");
        string password = ReadNonEmpty("Password: ");

        var result = _authService.Register(username, password);
        Console.WriteLine(result.Message);
    }

    private void Login()
    {
        PrintTitle("Login");
        string username = ReadNonEmpty("Username: ");
        string password = ReadNonEmpty("Password: ");

        var result = _authService.Login(username, password);
        Console.WriteLine(result.Message);
    }

    private void Logout()
    {
        _authService.Logout();
        Console.WriteLine("You have been logged out.");
    }

    private void AddBook()
    {
        PrintTitle("Add book");
        string title = ReadNonEmpty("Title: ");
        string author = ReadNonEmpty("Author: ");

        Console.Write("Year: ");
        if (!int.TryParse(Console.ReadLine(), out int year))
        {
            Console.WriteLine("Year must be a number.");
            return;
        }

        Console.WriteLine("Genres:");
        foreach (var genre in _bookService.GetGenres())
        {
            Console.WriteLine($"  [{genre.Id}] {genre.Name}");
        }
        Console.Write("Genre Id: ");
        if (!int.TryParse(Console.ReadLine(), out int genreId))
        {
            Console.WriteLine("The genre Id must be a number.");
            return;
        }

        ReadingStatus status = ReadStatus();

        var result = _bookService.AddBook(title, author, year, genreId, status);
        Console.WriteLine(result.Message);
    }

    private void ListBooks()
    {
        PrintTitle("All books");
        var books = _bookService.GetMyBooks();

        if (books.Count == 0)
        {
            Console.WriteLine("There are no books yet.");
            return;
        }

        foreach (var book in books)
        {
            PrintBook(book);
        }
    }

    private void FindBooks()
    {
        PrintTitle("Find book");
        string text = ReadNonEmpty("Search by title or author: ");
        var books = _bookService.FindMyBooks(text);

        if (books.Count == 0)
        {
            Console.WriteLine("No books found.");
            return;
        }

        foreach (var book in books)
        {
            PrintBook(book);
        }
    }

    private void DeleteBook()
    {
        PrintTitle("Delete book");
        Console.Write("Enter the Id of the book to delete: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("The Id must be a number.");
            return;
        }

        var result = _bookService.DeleteBook(id);
        Console.WriteLine(result.Message);
    }

    private void ShowStatistics()
    {
        PrintTitle("Books per genre");
        var stats = _bookService.GetStatistics();

        if (stats.Count == 0)
        {
            Console.WriteLine("There are no books yet.");
            return;
        }

        foreach (var item in stats)
        {
            Console.WriteLine($"{item.Genre}: {item.Total}");
        }
    }

    private static void PrintTitle(string text)
    {
        Console.WriteLine();
        Console.WriteLine($"==== {text} ====");
    }

    private static void PrintBook(Book book)
    {
        Console.WriteLine($"[{book.Id}] {book.Title} - {book.Author} ({book.Year}) | {book.GenreName} | {StatusText(book.Status)}");
    }

    private static string StatusText(ReadingStatus status)
    {
        switch (status)
        {
            case ReadingStatus.ToRead:
                return "To read";
            case ReadingStatus.Reading:
                return "Reading";
            default:
                return "Read";
        }
    }

    private static string ReadNonEmpty(string message)
    {
        while (true)
        {
            Console.Write(message);
            string input = (Console.ReadLine() ?? "").Trim();

            if (input != "")
            {
                return input;
            }
            Console.WriteLine("This field cannot be empty.");
        }
    }

    private static ReadingStatus ReadStatus()
    {
        while (true)
        {
            Console.WriteLine("Status: 1) To read  2) Reading  3) Read");
            Console.Write("Choose: ");
            string input = Console.ReadLine() ?? "";

            if (input == "1") return ReadingStatus.ToRead;
            if (input == "2") return ReadingStatus.Reading;
            if (input == "3") return ReadingStatus.Read;

            Console.WriteLine("Invalid status. Choose 1, 2 or 3.");
        }
    }
}