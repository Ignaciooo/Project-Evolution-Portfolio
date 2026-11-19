using System.ComponentModel.DataAnnotations;

namespace Milestone05_WebAPI.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
}

public class CreateUserDto
{
    [Required, StringLength(30, MinimumLength = 3)]
    public string Username { get; set; } = "";

    [Required, StringLength(100, MinimumLength = 4)]
    public string Password { get; set; } = "";
}