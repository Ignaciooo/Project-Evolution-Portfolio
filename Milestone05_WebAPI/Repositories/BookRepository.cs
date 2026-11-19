using Dapper;
using Milestone05_WebAPI.Data;
using Milestone05_WebAPI.Models;

namespace Milestone05_WebAPI.Repositories;

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

    public List<Book> GetAll(int? userId, string? pattern)
    {
        using var connection = _database.CreateConnection();
        return connection.Query<Book>(
            SelectBooks + @" WHERE (@UserId IS NULL OR b.UserId = @UserId)
                             AND (@Pattern IS NULL OR b.Title LIKE @Pattern OR b.Author LIKE @Pattern)
                             ORDER BY b.Id",
            new { UserId = userId, Pattern = pattern }).ToList();
    }

    public Book? GetById(int id)
    {
        using var connection = _database.CreateConnection();
        return connection.QuerySingleOrDefault<Book>(
            SelectBooks + " WHERE b.Id = @Id",
            new { Id = id });
    }

    public int Add(Book book)
    {
        using var connection = _database.CreateConnection();
        return connection.ExecuteScalar<int>(
            @"INSERT INTO Books (UserId, GenreId, Title, Author, Year, Status)
              VALUES (@UserId, @GenreId, @Title, @Author, @Year, @Status);
              SELECT last_insert_rowid();",
            new { book.UserId, book.GenreId, book.Title, book.Author, book.Year, book.Status });
    }

    public bool Update(Book book)
    {
        using var connection = _database.CreateConnection();
        int rows = connection.Execute(
            @"UPDATE Books
              SET GenreId = @GenreId, Title = @Title, Author = @Author, Year = @Year, Status = @Status
              WHERE Id = @Id",
            new { book.Id, book.GenreId, book.Title, book.Author, book.Year, book.Status });
        return rows > 0;
    }

    public bool Delete(int id)
    {
        using var connection = _database.CreateConnection();
        int rows = connection.Execute("DELETE FROM Books WHERE Id = @Id", new { Id = id });
        return rows > 0;
    }

    public bool TitleExists(int userId, string title, int? excludeId)
    {
        using var connection = _database.CreateConnection();
        int count = connection.ExecuteScalar<int>(
            @"SELECT COUNT(1) FROM Books
              WHERE UserId = @UserId AND Title = @Title
              AND (@ExcludeId IS NULL OR Id <> @ExcludeId)",
            new { UserId = userId, Title = title, ExcludeId = excludeId });
        return count > 0;
    }
}