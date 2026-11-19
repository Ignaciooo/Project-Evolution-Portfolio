using System.Security.Cryptography;
using System.Text;
using Milestone04_Database.Models;
using Milestone04_Database.Repositories;

namespace Milestone04_Database.Services;

public class AuthService
{
    private readonly UserRepository _userRepository;

    public User? CurrentUser { get; private set; }

    public bool IsLoggedIn => CurrentUser != null;

    public AuthService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public (bool Success, string Message) Register(string username, string password)
    {
        username = username.Trim();

        if (username.Length < 3)
        {
            return (false, "Username must have at least 3 characters.");
        }
        if (username.Contains(' '))
        {
            return (false, "Username cannot contain spaces.");
        }
        if (password.Length < 4)
        {
            return (false, "Password must have at least 4 characters.");
        }
        if (_userRepository.FindByUsername(username) != null)
        {
            return (false, "That username is already taken.");
        }

        var user = new User
        {
            Username = username,
            PasswordHash = HashPassword(password)
        };
        _userRepository.Add(user);

        return (true, "User registered successfully. You can now log in.");
    }

    public (bool Success, string Message) Login(string username, string password)
    {
        var user = _userRepository.FindByUsername(username.Trim());

        if (user == null || user.PasswordHash != HashPassword(password))
        {
            return (false, "Wrong username or password.");
        }

        CurrentUser = user;
        return (true, $"Welcome, {user.Username}!");
    }

    public void Logout()
    {
        CurrentUser = null;
    }

    private static string HashPassword(string password)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }
}
