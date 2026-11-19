using Dapper;
using Milestone05_WebAPI.Data;
using Milestone05_WebAPI.Models;

namespace Milestone05_WebAPI.Repositories;

public class UserRepository
{
    private readonly DatabaseContext _database;

    public UserRepository(DatabaseContext database)
    {
        _database = database;
    }

    public User? FindById(int id)
    {
        using var connection = _database.CreateConnection();
        return connection.QuerySingleOrDefault<User>(
            "SELECT Id, Username, PasswordHash FROM Users WHERE Id = @Id",
            new { Id = id });
    }

    public User? FindByUsername(string username)
    {
        using var connection = _database.CreateConnection();
        return connection.QuerySingleOrDefault<User>(
            "SELECT Id, Username, PasswordHash FROM Users WHERE Username = @Username",
            new { Username = username });
    }

    public int Add(User user)
    {
        using var connection = _database.CreateConnection();
        return connection.ExecuteScalar<int>(
            "INSERT INTO Users (Username, PasswordHash) VALUES (@Username, @PasswordHash); SELECT last_insert_rowid();",
            new { user.Username, user.PasswordHash });
    }
}