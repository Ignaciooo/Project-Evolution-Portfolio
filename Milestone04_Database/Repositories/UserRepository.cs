using Dapper;
using Milestone04_Database.Data;
using Milestone04_Database.Models;

namespace Milestone04_Database.Repositories;

public class UserRepository
{
    private readonly DatabaseContext _database;

    public UserRepository(DatabaseContext database)
    {
        _database = database;
    }

    public User? FindByUsername(string username)
    {
        using var connection = _database.CreateConnection();
        return connection.QuerySingleOrDefault<User>(
            "SELECT Id, Username, PasswordHash FROM Users WHERE Username = @Username",
            new { Username = username });
    }

    public void Add(User user)
    {
        using var connection = _database.CreateConnection();
        connection.Execute(
            "INSERT INTO Users (Username, PasswordHash) VALUES (@Username, @PasswordHash)",
            new { user.Username, user.PasswordHash });
    }
}