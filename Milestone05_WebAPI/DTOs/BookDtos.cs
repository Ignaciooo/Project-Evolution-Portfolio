using System.ComponentModel.DataAnnotations;
using Milestone05_WebAPI.Models;

namespace Milestone05_WebAPI.DTOs;

public class BookDto
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

public class CreateBookDto
{
    [Range(1, int.MaxValue)]
    public int UserId { get; set; }

    [Required, StringLength(200)]
    public string Title { get; set; } = "";

    [Required, StringLength(100)]
    public string Author { get; set; } = "";

    [Range(1, 9999)]
    public int Year { get; set; }

    [Range(1, int.MaxValue)]
    public int GenreId { get; set; }

    [EnumDataType(typeof(ReadingStatus))]
    public ReadingStatus Status { get; set; }
}

public class UpdateBookDto
{
    [Required, StringLength(200)]
    public string Title { get; set; } = "";

    [Required, StringLength(100)]
    public string Author { get; set; } = "";

    [Range(1, 9999)]
    public int Year { get; set; }

    [Range(1, int.MaxValue)]
    public int GenreId { get; set; }

    [EnumDataType(typeof(ReadingStatus))]
    public ReadingStatus Status { get; set; }
}