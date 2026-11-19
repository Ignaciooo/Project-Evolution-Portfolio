using Dapper;
using Microsoft.Data.Sqlite;

namespace Milestone05_WebAPI.Data;

public class DatabaseContext
{
    private readonly string _connectionString;

    public DatabaseContext(string filePath)
    {
        _connectionString = $"Data Source={filePath};Foreign Keys=True";
    }

    public SqliteConnection CreateConnection()
    {
        return new SqliteConnection(_connectionString);
    }

    public void Initialize()
    {
        string schemaPath = Path.Combine(AppContext.BaseDirectory, "Data", "schema.sql");
        string schema = File.ReadAllText(schemaPath);

        using var connection = CreateConnection();
        connection.Execute(schema);
    }
}