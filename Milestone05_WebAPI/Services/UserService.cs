using System.Security.Cryptography;
using System.Text;
using Milestone05_WebAPI.DTOs;
using Milestone05_WebAPI.Models;
using Milestone05_WebAPI.Repositories;

namespace Milestone05_WebAPI.Services;

public class UserService
{
    private readonly UserRepository _userRepository;

    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public ServiceResult<UserDto> Register(CreateUserDto dto)
    {
        string username = dto.Username.Trim();

        if (username.Length < 3)
        {
            return ServiceResult<UserDto>.Fail(ErrorType.Validation, "Username must have at least 3 characters.");
        }
        if (username.Contains(' '))
        {
            return ServiceResult<UserDto>.Fail(ErrorType.Validation, "Username cannot contain spaces.");
        }
        if (_userRepository.FindByUsername(username) != null)
        {
            return ServiceResult<UserDto>.Fail(ErrorType.Conflict, "That username is already taken.");
        }

        var user = new User
        {
            Username = username,
            PasswordHash = HashPassword(dto.Password)
        };
        int id = _userRepository.Add(user);

        return ServiceResult<UserDto>.Ok(new UserDto { Id = id, Username = username });
    }

    public ServiceResult<UserDto> GetUser(int id)
    {
        var user = _userRepository.FindById(id);
        if (user == null)
        {
            return ServiceResult<UserDto>.Fail(ErrorType.NotFound, "User not found.");
        }
        return ServiceResult<UserDto>.Ok(new UserDto { Id = user.Id, Username = user.Username });
    }

    private static string HashPassword(string password)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }
}