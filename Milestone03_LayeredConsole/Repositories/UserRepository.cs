using Milestone03_LayeredConsole.Models;

namespace Milestone03_LayeredConsole.Repositories;

public class UserRepository
{
    private readonly string _filePath;
    private readonly List<User> _users;

    public UserRepository(string filePath)
    {
        _filePath = filePath;
        _users = JsonFile.Load<User>(filePath);
    }

    public User? FindByUsername(string username)
    {
        foreach (var user in _users)
        {
            if (string.Equals(user.Username, username, StringComparison.OrdinalIgnoreCase))
            {
                return user;
            }
        }
        return null;
    }

    public User Add(User user)
    {
        user.Id = GetNextId();
        _users.Add(user);
        JsonFile.Save(_filePath, _users);
        return user;
    }

    private int GetNextId()
    {
        int maxId = 0;
        foreach (var user in _users)
        {
            if (user.Id > maxId)
            {
                maxId = user.Id;
            }
        }
        return maxId + 1;
    }
}