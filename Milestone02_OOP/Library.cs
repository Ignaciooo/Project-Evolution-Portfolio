using System.Text.Json;
using System.Text.Json.Serialization;
public class Library
{
    private readonly List<Book> _books;
    private readonly string _filePath;
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() } // saves "Read" instead of 2
    };

    public Library(string filePath)
    {
        _filePath = filePath;
        _books = Load();
    }

    public IReadOnlyList<Book> Books => _books;

    public Book Add(string title, string author, int year, ReadingStatus status)
    {
        var book = new Book(GetNextId(), title, author, year, status);
        _books.Add(book);
        Save();
        return book;
    }

    public bool TitleExists(string title)
    {
        foreach (var book in _books)
        {
            if (string.Equals(book.Title, title.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }

    public List<Book> Find(string text)
    {
        var result = new List<Book>();
        foreach (var book in _books)
        {
            if (book.Title.Contains(text, StringComparison.OrdinalIgnoreCase) ||
                book.Author.Contains(text, StringComparison.OrdinalIgnoreCase))
            {
                result.Add(book);
            }
        }
        return result;
    }

    public bool Remove(int id)
    {
        Book? bookToRemove = null;
        foreach (var book in _books)
        {
            if (book.Id == id)
            {
                bookToRemove = book;
                break;
            }
        }

        if (bookToRemove == null)
        {
            return false;
        }

        _books.Remove(bookToRemove);
        Save();
        return true;
    }

    private int GetNextId()
    {
        int maxId = 0;
        foreach (var book in _books)
        {
            if (book.Id > maxId)
            {
                maxId = book.Id;
            }
        }
        return maxId + 1;
    }
    private List<Book> Load()
    {
        if (!File.Exists(_filePath))
        {
            return new List<Book>();
        }

        try
        {
            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Book>>(json, _options) ?? new List<Book>();
        }
        catch (Exception ex) when (ex is JsonException || ex is ArgumentException)
        {
            Console.WriteLine("Could not read books.json. Starting with an empty library.");
            return new List<Book>();
        }
    }

    private void Save()
    {
        string json = JsonSerializer.Serialize(_books, _options);
        File.WriteAllText(_filePath, json);
    }
}