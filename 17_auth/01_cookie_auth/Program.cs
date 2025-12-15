using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication("MyCookie")
    .AddCookie("MyCookie", options =>
    {
        options.Cookie.Name = "AuthCookie";
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// ВАЖЕН ПОРЯДОК!!!
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", (HttpContext ctx) => 
{
    return ctx.User.Identity?.IsAuthenticated == true ?
        $"Hello {ctx.User.Identity.Name}!" :
        "Hello noname!";
});

app.MapGet("/login", () => 
{
    var filePath = Path.Combine(app.Environment.ContentRootPath, "pages", "login.html");

    return Results.File(filePath, "text/html");
});

app.MapPost("/login", async (HttpContext ctx) => 
{
    const string validEmail = "vasia@mail.com";
    const string validPassword = "qwerty123";

    var req = ctx.Request;

    if (req.Form["email"] == validEmail && req.Form["password"] == validPassword)
    {
        var claims = new List<Claim>
        {
            new (ClaimTypes.Name, "Vasia"),
            new (ClaimTypes.Email, "vasia@mail.com"),
            new (ClaimTypes.Role, "User")
        };

        var identity = new ClaimsIdentity(claims, "MyCookie");
        var principal = new ClaimsPrincipal(identity);

        await ctx.SignInAsync("MyCookie", principal);           // <-- LOGIN

        return Results.Redirect("/");
    }

    return Results.Unauthorized();
});

app.Run();
