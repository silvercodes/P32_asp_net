#region Use()

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.Use(async (ctx, next) =>
//{
//    // Логика до передачи в next

//    // Вызов next
//    // await next.Invoke(ctx);         // next --> RequestDelegate
//    // OR
//    await next();                       // next --> Func<Task>

//    // Логика после передачи в next
//});

//app.Run();





//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//string time = string.Empty;

//app.Use(async (ctx, next) =>
//{
//    time = DateTime.Now.ToShortTimeString();

//    await next();

//    Console.WriteLine($"time: {time}");
//});

//app.Run(async (ctx) =>
//{
//    await ctx.Response.WriteAsync($"time: {time}");
//});

//app.Run();




// Плохой пример !!!
//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.Use(async (ctx, next) =>
//{
//    await ctx.Response.WriteAsync("Hello");
//    await next();
//    await ctx.Response.WriteAsync("Buy");
//});

//app.Run(async ctx =>
//{
//    await ctx.Response.WriteAsync("Vasia");
//});

//app.Run();




//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.Use(async (ctx, next) =>
//{
//    string? path = ctx.Request.Path.Value;
//    if (path == "/time")
//        await ctx.Response.WriteAsync($"{DateTime.Now.ToShortTimeString()}");
//    else
//        await next();
//});

//app.Run(async ctx =>
//{
//    await ctx.Response.WriteAsync("Terminal MW");
//});

//app.Run();



// --- отдельный метод c Func<Task>
//async Task GetTime(HttpContext ctx, Func<Task> next)
//{
//    string? path = ctx.Request.Path.Value;
//    if (path == "/time")
//        await ctx.Response.WriteAsync($"{DateTime.Now.ToShortTimeString()}");
//    else
//        await next();
//}

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.Use(GetTime);

//app.Run(async ctx =>
//{
//    await ctx.Response.WriteAsync("Terminal MW");
//});

//app.Run();




// --- отдельный метод c RequestDelegate
//async Task GetTime(HttpContext ctx, RequestDelegate next)
//{
//    string? path = ctx.Request.Path.Value;
//    if (path == "/time")
//        await ctx.Response.WriteAsync($"{DateTime.Now.ToShortTimeString()}");
//    else
//        await next.Invoke(ctx);
//}

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.Use(GetTime);

//app.Run(async ctx =>
//{
//    await ctx.Response.WriteAsync("Terminal MW");
//});

//app.Run();

#endregion

#region UseWhen() / MapWhen()

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseWhen(
    ctx => ctx.Request.Path == "/time",
    app =>
    {
        app.Use(...);
        app.Use(...);
    }
);

app.Run(async ctx =>
{
    await ctx.Response.WriteAsync("Terminal MW");
});

app.Run();




#endregion
