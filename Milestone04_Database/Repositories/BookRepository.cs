using Dapper;
using Milestone04_Database.Data;
using Milestone04_Database.Models;

namespace Milestone04_Database.Repositories;

public class BookRepository
{
    private const string SelectBooks = @"
        SELECT b.Id, b.UserId, b.GenreId, g.Name AS GenreName,
               b.Title, b.Author, b.Year, b.Status
        FROM Books b
        JOIN Genres g ON g.Id = b.GenreId";

    private readonly DatabaseContext _database;

    public BookRepository(DatabaseContext database)
    {
        _database = database;
    }

    public void Add(Book book)
    {
        using var connection = _database.CreateConnection();
        connection.Execute(
            @"INSERT INTO Books (UserId, GenreId, Title, Author, Year, Status)
              VALUES (@UserId, @GenreId, @Title, @Author, @Year, @Status)",
            new { book.UserId, book.GenreId, book.Title, book.Author, book.Year, book.Status });
    }

    public List<Book> GetByUser(int userId)
    {
        using var connection = _database.CreateConnection();
        return connection.Query<Book>(
            SelectBooks + " WHERE b.UserId = @UserId ORDER BY b.Title",
            new { UserId = userId }).ToList();
    }

    public List<Book> Search(int userId, string text)
    {
        using var connection = _database.CreateConnection();
        return connection.Query<Book>(
            SelectBooks + @" WHERE b.UserId = @UserId
                             AND (b.Title LIKE @Pattern OR b.Author LIKE @Pattern)
                             ORDER BY b.Title",
            new { UserId = userId, Pattern = "%" + text + "%" }).ToList();
    }

    public bool TitleExists(int userId, string title)
    {
        using var connection = _database.CreateConnection();
        int count = connection.ExecuteScalar<int>(
            "SELECT COUNT(1) FROM Books WHERE UserId = @UserId AND Title = @Title",
            new { UserId = userId, Title = title });
        return count > 0;
    }

    public bool Delete(int id, int userId)
    {
        using var connection = _database.CreateConnection();
        int rows = connection.Execute(
            "DELETE FROM Books WHERE Id = @Id AND UserId = @UserId",
            new { Id = id, UserId = userId });
        return rows > 0;
    }

    public List<GenreCount> CountByGenre(int userId)
    {
        using var connection = _database.CreateConnection();
        return connection.Query<GenreCount>(
            @"SELECT g.Name AS Genre, COUNT(b.Id) AS Total
              FROM Books b
              JOIN Genres g ON g.Id = b.GenreId
              WHERE b.UserId = @UserId
              GROUP BY g.Name
              ORDER BY Total DESC",
            new { UserId = userId }).ToList();
    }
}