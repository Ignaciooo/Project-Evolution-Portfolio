namespace Milestone03_LayeredConsole.Models;

public class Book
{
    public int Id { get; set; }
    public int OwnerId { get; set; }
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public int Year { get; set; }
    public ReadingStatus Status { get; set; }
}
