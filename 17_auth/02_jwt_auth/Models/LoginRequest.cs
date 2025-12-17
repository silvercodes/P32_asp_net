namespace _02_jwt_auth.Models;

public class LoginRequest
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}
