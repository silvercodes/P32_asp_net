using rest_api.Models.Auth;

namespace rest_api.Services;

public interface IAuthService
{
    Task<AuthResponse> Register(RegisterRequest request);
    Task<AuthResponse> Login(LoginRequest request);
}
