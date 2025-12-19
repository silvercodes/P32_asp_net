using _03_role_authorization.Models;

namespace _03_role_authorization.Services;

public class UserService
{
    private readonly List<User> users =
    [
        new User 
        {
            Username = "petya", 
            Password = "123123123", 
            Roles = new List<string>{ "User" } },
        new User 
        {
            Username = "vasia", 
            Password = "123123123", 
            Roles = new List<string> { "Admin", "Support" } 
        },
        new User
        {
            Username = "kolya",
            Password = "123123123",
            Roles = new List<string> { "Support" }
        },
    ];

    public User? ValidateUser(string username, string password)
    {
        return users.FirstOrDefault(u =>
            u.Username == username &&
            u.Password == password);
    }
}