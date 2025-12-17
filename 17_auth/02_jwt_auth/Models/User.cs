namespace _02_jwt_auth.Models;

public class User
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Role { get; set; }
}
