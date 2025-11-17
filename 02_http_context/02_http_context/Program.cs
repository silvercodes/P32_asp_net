//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.Run(async ctx =>
//{
//    HttpRequest req = ctx.Request;

//    var method = req.Method;
//    var path = req.Path;
//    var id = req.Query["id"];
//    var userAgent = req.Headers["User-Agent"];

//    Console.WriteLine($"{method} {path} {id} {userAgent}");
//});

//app.Run();



#region HttpResponse

//using System.Text.Json;

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

// -- base example
//app.Run(async ctx =>
//{
//    HttpResponse res = ctx.Response;

//    res.ContentType = "text/plain";
//    res.StatusCode = 201;

//    await res.WriteAsync("Hello from server");
//    //
//    await res.WriteAsync("\nVasia");
//});

//app.Run();

// -- Set custom headers / status codes

//app.Run(async ctx =>
//{
//    HttpResponse res = ctx.Response;

//    res.StatusCode = 418;
//    res.Headers["X-Api-Version"] = "2.0";

//    await res.WriteAsync("Hello from teapot");
//});

//app.Run();

// -- Send HTML --

//app.Run(async ctx =>
//{
//    HttpResponse res = ctx.Response;
//    res.StatusCode = 200;
//    res.ContentType = "text/html; charset=utf-8";

//    await res.WriteAsync($@"
//        <!DOCTYPE html>
//        <html>
//        <head>
//            <title>HTML</title>
//        </head>
//        <body>
//            <h1>Hello</h1>
//            <p>Current time: {DateTime.Now.ToShortTimeString()}</p>
//        </body>
//        </html>
//    ");
//});

//app.Run();



// --- redirect ---

//app.Run(async ctx =>
//{
//    HttpResponse res = ctx.Response;

//    res.Redirect("https://google.com", true);

//});

//app.Run();


// --- json ---
//app.Run(async ctx =>
//{
//    HttpResponse res = ctx.Response;

//    res.ContentType = "application/json; charset=utf-8";

//    var data = new { Name = "Petya", Age = 23 };

//    await res.WriteAsync(JsonSerializer.Serialize(data));

//});

//app.Run();



// --- отправка файлов ---

//app.Run(async ctx =>
//{
//    HttpResponse res = ctx.Response;

//    res.ContentType = "text/plain";
//    res.Headers.ContentDisposition = "attachment; filename=serverData.txt";

//    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "data.txt");

//    await res.SendFileAsync(filePath);
//});

//app.Run();



// --- потоковая передача ---

//app.Run(async ctx =>
//{
//    HttpResponse res = ctx.Response;

//    for (int i = 0; i < 5; ++i)
//    {
//        await res.WriteAsync($"DATA: {i}");
//        await res.Body.FlushAsync();
//        await Task.Delay(1000);
//    }
//});

//app.Run();




// --- Simple Routing ---

//app.Run(async ctx =>
//{
//    HttpRequest req = ctx.Request;
//    HttpResponse res = ctx.Response;

//    res.ContentType = "text/plain; charset=utf-8";

//    if (req.Path == "/time")
//        await res.WriteAsync($"{DateTime.Now.ToShortTimeString()}");
//    else if (req.Path == "/date")
//        await res.WriteAsync($"{DateTime.Now.ToShortDateString()}");
//    else
//    {
//        res.StatusCode = 404;
//        await res.WriteAsync("Page not found");
//    }
//});

//app.Run();

#endregion

#region HttpRequest

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//app.Run(async ctx =>
//{
//    HttpRequest req = ctx.Request;
//    HttpResponse res = ctx.Response;

//    var report = new 
//    { 
//        Method = req.Method,
//        Path = req.Path,
//        Protocol = req.Protocol,
//        Host = req.Host,
//        QueryString = req.QueryString,
//        ContentType = req.ContentType,
//        ContentLength = req.ContentLength,
//    };

//    await res.WriteAsJsonAsync(report);
//});

//app.Run();



//app.Run(async ctx =>
//{
//    HttpRequest req = ctx.Request;
//    HttpResponse res = ctx.Response;

//    string name = req.Query["name"].ToString();

//    string sort = req.Query.TryGetValue("sort", out var f) ?
//                        f.ToString().ToLower() :
//                        "id";

//    await res.WriteAsJsonAsync(new { name, sort });
//});

//app.Run();




//app.Run(async ctx =>
//{
//    HttpRequest req = ctx.Request;
//    HttpResponse res = ctx.Response;

//    if (!req.Headers.TryGetValue("X-Api_Key", out var apiKey))
//    {
//        res.StatusCode = StatusCodes.Status401Unauthorized;
//        await res.WriteAsync("API key is required");
//        return;
//    }

//    if (!req.Cookies.TryGetValue("auth-token", out var token))
//    {
//        res.StatusCode = StatusCodes.Status403Forbidden;
//        await res.WriteAsync("Authorization failed");
//        return;
//    }

//    await res.WriteAsync("Access granted");
//});

//app.Run();




app.Run(async ctx =>
{
    HttpRequest req = ctx.Request;
    HttpResponse res = ctx.Response;

    switch(req.Path)
    {
        case "/api/v1/users" when req.Method == "GET":

            break;
        case "/api/v1/users" when req.Method == "POST":

            break;
        default:
            res.StatusCode = StatusCodes.Status404NotFound;
            await res.WriteAsync("Endpoint not found");
            break;
    }
});

app.Run();

#endregion




