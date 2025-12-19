namespace _03_role_authorization.Models;

public class LoginRequest
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}