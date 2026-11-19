using System.Text.Json.Serialization;

public enum ReadingStatus
{
    ToRead,
    Reading,
    Read
}

public class Book
{
    private string _title = "";
    private string _author = "";
    private int _year;

    public int Id { get; }

    public string Title
    {
        get => _title;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Title cannot be empty.");
            }
            _title = value.Trim();
        }
    }

    public string Author
    {
        get => _author;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Author cannot be empty.");
            }
            _author = value.Trim();
        }
    }

    public int Year
    {
        get => _year;
        set
        {
            if (value < 1 || value > DateTime.Now.Year)
            {
                throw new ArgumentException($"Year must be between 1 and {DateTime.Now.Year}.");
            }
            _year = value;
        }
    }

    public ReadingStatus Status { get; set; }

    [JsonConstructor]
    public Book(int id, string title, string author, int year, ReadingStatus status)
    {
        Id = id;
        Title = title;
        Author = author;
        Year = year;
        Status = status;
    }

    public string GetStatusText()
    {
        switch (Status)
        {
            case ReadingStatus.ToRead:
                return "To read";
            case ReadingStatus.Reading:
                return "Reading";
            default:
                return "Read";
        }
    }

    public override string ToString()
    {
        return $"[{Id}] {Title} - {Author} ({Year}) | {GetStatusText()}";
    }
}