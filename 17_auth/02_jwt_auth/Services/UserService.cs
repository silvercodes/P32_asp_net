using _02_jwt_auth.Models;

namespace _02_jwt_auth.Services;

public class UserService
{
    private readonly List<User> users =
    [
        new User {Username = "petya", Password = "123123123", Role = "User"},
        new User {Username = "vasia", Password = "123123123", Role = "Admin"},
    ];

    public User? ValidateUser(string username, string password)
    {
        return users.FirstOrDefault(u => 
            u.Username == username &&
            u.Password == password);
    }
}
