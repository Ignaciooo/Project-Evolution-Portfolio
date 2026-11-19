namespace Milestone05_WebAPI.Models;

public class Book
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int GenreId { get; set; }
    public string GenreName { get; set; } = "";
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public int Year { get; set; }
    public ReadingStatus Status { get; set; }
}