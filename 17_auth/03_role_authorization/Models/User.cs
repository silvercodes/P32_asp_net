namespace _03_role_authorization.Models;

public class User
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required List<string> Roles { get; set; }
}
