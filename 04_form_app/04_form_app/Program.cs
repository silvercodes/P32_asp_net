using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using _04_form_app.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var env = app.Environment;

app.UseStaticFiles();

app.Run(async ctx =>
{
    var req = ctx.Request;
    var res = ctx.Response;

    switch(req.Path)
    {
        case "/signup" when req.Method == "GET":
            try
            {
                var templatePath = Path.Combine(env.ContentRootPath, "Templates", "register-page.html");
                ctx.Response.ContentType = "text/html; charset=utf-8;";
                await ctx.Response.SendFileAsync(templatePath);
            }
            catch (Exception)
            {
                await SendError(res, 500, "Registration form is unvaliable");
            }
            break;

        case "/signup" when req.Method == "POST":
            try
            {
                if (! req.HasFormContentType)
                {
                    await SendError(res, 415, "Unsupported Media Type");
                    return;
                }

                var form = await req.ReadFormAsync();

                // TODO: validation

                var user = new User()
                {
                    Id = Guid.NewGuid(),
                    Username = form["username"].ToString(),
                    Email = form["email"].ToString(),
                    PasswordHash = HashPassword(form["passwod"].ToString()),
                    CreatedAt = DateTime.UtcNow,
                };

                Console.WriteLine(JsonSerializer.Serialize(user));





            }
            catch (Exception)
            {

                throw;
            }
            break;
    }
});



app.Run();

async Task SendError(HttpResponse res, int statusCode, string message)
{
    res.StatusCode = statusCode;
    // res.ContentType = "application/json; charset=utf-8;";
    await res.WriteAsJsonAsync(new { Error = message });
}

string HashPassword(string password)
{
    using var sha = SHA256.Create();
    var bytes = Encoding.UTF8.GetBytes(password);
    var hash = sha.ComputeHash(bytes);

    return Convert.ToBase64String(hash);
}

