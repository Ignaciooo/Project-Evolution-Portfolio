using Dapper;
using Milestone04_Database.Data;
using Milestone04_Database.Models;

namespace Milestone04_Database.Repositories;

public class GenreRepository
{
    private readonly DatabaseContext _database;

    public GenreRepository(DatabaseContext database)
    {
        _database = database;
    }

    public List<Genre> GetAll()
    {
        using var connection = _database.CreateConnection();
        return connection.Query<Genre>("SELECT Id, Name FROM Genres ORDER BY Id").ToList();
    }

    public Genre? GetById(int id)
    {
        using var connection = _database.CreateConnection();
        return connection.QuerySingleOrDefault<Genre>(
            "SELECT Id, Name FROM Genres WHERE Id = @Id",
            new { Id = id });
    }
}