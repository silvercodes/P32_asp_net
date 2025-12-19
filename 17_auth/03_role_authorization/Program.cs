using System.Security.Claims;
using System.Text;
using _03_role_authorization.Models;
using _03_role_authorization.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<JwtService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // --> 401
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("SupportOnly", policy => policy.RequireRole("Support"));

    // OR
    options.AddPolicy("AdminOrSupport", policy => policy.RequireRole("Admin", "Support"));

    // AND
    options.AddPolicy("AdminAndSupport", policy => 
        policy
            .RequireRole("Admin")
            .RequireRole("Support")
        );
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/login", (
    [FromBody] LoginRequest req,
    UserService userService,
    JwtService jwt
) =>
{
    User? user = userService.ValidateUser(req.Username, req.Password);

    if (user is null)
        return Results.Unauthorized(); // --> 401

    var token = jwt.GenerateToken(user);

    return Results.Ok(new LoginResponse { Token = token });
}).AllowAnonymous();

app.MapGet("/user-only", () => "Authenticated users only")
    .RequireAuthorization();

app.MapGet("/admin-only", () => "Admin only")
    .RequireAuthorization("AdminOnly");

app.MapGet("/support-only", () => "Supprt only")
    .RequireAuthorization("SupportOnly");

app.MapGet("/admin-or-support", () => "Admin or Support")
    .RequireAuthorization("AdminOrSupport");

app.MapGet("/admin-and-support", () => "Admin and Support")
    .RequireAuthorization("AdminAndSupport");

app.MapGet("/user-info", (HttpContext ctx) =>
{
    var user = ctx.User;

    return new
    {
        user.Identity?.Name,
        Roles = user.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
    };
}).RequireAuthorization();

app.Run();
