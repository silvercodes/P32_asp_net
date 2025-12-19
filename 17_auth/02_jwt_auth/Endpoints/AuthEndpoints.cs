using _02_jwt_auth.Models;
using _02_jwt_auth.Services;
using Microsoft.AspNetCore.Mvc;

namespace _02_jwt_auth.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var authGroup = app.MapGroup("/auth");

        authGroup.MapPost("/login", (
            [FromBody] LoginRequest req,
            UserService userService,
            JwtService jwt) => 
        {
            User? user = userService.ValidateUser(req.Username, req.Password);

            if (user is null)
                return Results.Unauthorized(); // --> 401

            var token = jwt.GenerateToken(user);

            return Results.Ok(new LoginResponse { Token = token });
        }).AllowAnonymous();

        authGroup.MapGet("/protected", () => "This ISDATE protected endpoint (Jwt token only)")
            .RequireAuthorization();

        authGroup.MapGet("/admin", () => "This is admin only data")
            .RequireAuthorization(policy => policy.RequireRole("Admin"));
    }
}
