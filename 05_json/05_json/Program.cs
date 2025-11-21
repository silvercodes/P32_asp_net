using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var jsonOptions = new JsonSerializerOptions()
{
    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
    WriteIndented = true,
};

//app.Run(async ctx =>
//{
//    if (ctx.Request.Path == "/test")
//    {
//        var data = new
//        {
//            Message = "Hello from server",
//            Timestamp = DateTime.UtcNow,
//        };

//        await ctx.Response.WriteAsJsonAsync(data, jsonOptions);
//    }
//});


app.Run(async ctx => 
{ 
    if (ctx.Request.Path == "/test" && ctx.Request.Method == "POST")
    {
        var user = await ctx.Request.ReadFromJsonAsync<User>();
        await ctx.Response.WriteAsync($"Tested: {user.Email}");
    }
});

app.Run();

class User
{
    public string Name { get; set; }
    public string Email { get; set; }
}
