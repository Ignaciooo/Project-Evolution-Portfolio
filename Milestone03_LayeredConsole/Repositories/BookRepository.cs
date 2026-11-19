using Milestone03_LayeredConsole.Models;

namespace Milestone03_LayeredConsole.Repositories;

public class BookRepository
{
    private readonly string _filePath;
    private readonly List<Book> _books;

    public BookRepository(string filePath)
    {
        _filePath = filePath;
        _books = JsonFile.Load<Book>(filePath);
    }

    public List<Book> GetByOwner(int ownerId)
    {
        var result = new List<Book>();
        foreach (var book in _books)
        {
            if (book.OwnerId == ownerId)
            {
                result.Add(book);
            }
        }
        return result;
    }

    public Book? GetById(int id)
    {
        foreach (var book in _books)
        {
            if (book.Id == id)
            {
                return book;
            }
        }
        return null;
    }

    public Book Add(Book book)
    {
        book.Id = GetNextId();
        _books.Add(book);
        JsonFile.Save(_filePath, _books);
        return book;
    }

    public void Remove(Book book)
    {
        _books.Remove(book);
        JsonFile.Save(_filePath, _books);
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
}