using System.Text.Json;
using System.Text.Json.Serialization;

namespace Milestone03_LayeredConsole.Repositories;

public static class JsonFile
{
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public static List<T> Load<T>(string path)
    {
        if (!File.Exists(path))
        {
            return new List<T>();
        }

        try
        {
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<T>>(json, Options) ?? new List<T>();
        }
        catch (JsonException)
        {
            return new List<T>();
        }
    }

    public static void Save<T>(string path, List<T> items)
    {
        string json = JsonSerializer.Serialize(items, Options);
        File.WriteAllText(path, json);
    }
}