namespace rest_api.Models.Auth;

public class AuthResponse
{
    public int UserId { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
}
