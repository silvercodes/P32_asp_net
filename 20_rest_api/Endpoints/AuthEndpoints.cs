using rest_api.Models.Auth;
using rest_api.Services;

using Microsoft.AspNetCore.Mvc;

namespace rest_api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var authGroup = app.MapGroup("/auth");

        authGroup.MapPost("/register", async (
            [FromBody] RegisterRequest request,
            IAuthService authService) =>
        {
            try
            {
                var response = await authService.Register(request);
                return Results.Ok(response);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }).AllowAnonymous();

        authGroup.MapPost("/login", async (
            [FromBody] LoginRequest request,
            IAuthService authService) =>
        {
            try
            {
                var response = await authService.Login(request);
                return Results.Ok(response);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }).AllowAnonymous();
    }
}
