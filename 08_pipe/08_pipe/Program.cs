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

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.UseWhen(
//    ctx => ctx.Request.Path == "/time",
//    app =>
//    {
//        app.Use(async(ctx, next) =>
//        {
//            var time = DateTime.Now.ToShortTimeString();
//            Console.WriteLine($"TIME: {time}");
//            await next();
//        });
//        app.Run(async ctx =>
//        {
//            var time = DateTime.Now.ToShortTimeString();
//            await ctx.Response.WriteAsync($"TIME: {time}");
//        });
//    }
//);

//app.Run(async ctx =>
//{
//    await ctx.Response.WriteAsync("Terminal MW");
//});

//app.Run();





//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.UseWhen(
//    ctx => ctx.Request.Path == "/time",
//    app =>
//    {
//        app.Use(async (ctx, next) =>
//        {
//            var time = DateTime.Now.ToShortTimeString();
//            Console.WriteLine($"TIME: {time}");
//            await next();
//        });
//        app.Use(async (ctx, next) =>
//        {
//            var time = DateTime.Now.ToShortTimeString();
//            await ctx.Response.WriteAsync($"TIME: {time}");
//            await next();
//        });
//    }
//);

//app.Run(async ctx =>
//{
//    await ctx.Response.WriteAsync("Terminal MW");
//});

//app.Run();






//app.UseWhen(
//    ctx => ctx.Request.Path == "/time",
//    BuildBranch
//);

//app.Run(async ctx =>
//{
//    await ctx.Response.WriteAsync("Terminal MW");
//});

//app.Run();

//void BuildBranch(IApplicationBuilder app)
//{
//    app.Use(async (ctx, next) =>
//    {
//        var time = DateTime.Now.ToShortTimeString();
//        Console.WriteLine($"TIME: {time}");
//        await next();
//    });
//    app.Use(async (ctx, next) =>
//    {
//        var time = DateTime.Now.ToShortTimeString();
//        await ctx.Response.WriteAsync($"TIME: {time}");
//        await next();
//    });
//}






//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.MapWhen(
//    ctx => ctx.Request.Path == "/time",
//    app =>
//    {
//        app.Run(async ctx =>
//        {
//            var time = DateTime.Now.ToShortTimeString();
//            await ctx.Response.WriteAsync($"TIME: {time}");
//        });
//    }
//);

//app.Run(async ctx =>
//{
//    await ctx.Response.WriteAsync("Terminal MW");
//});

//app.Run();


#endregion

#region Map()

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.Map("/time", app =>
//{
//    var time = DateTime.Now.ToShortTimeString();

//    app.Use(async (ctx, next) =>
//    {
//        Console.WriteLine($"TIME: {time}");
//        await next();
//    });
//    app.Run(async ctx => await ctx.Response.WriteAsync($"END"));
//});

//app.Run(async ctx =>
//{
//    await ctx.Response.WriteAsync("Terminal MW");
//});

//app.Run();





//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.Map("/time", app => app.Run(async ctx => await ctx.Response.WriteAsync($"{DateTime.Now.ToShortTimeString()}")));
//app.Map("/date", app => app.Run(async ctx => await ctx.Response.WriteAsync($"{DateTime.Now.ToShortDateString()}")));
//app.Map("/home", app => app.Run(async ctx => await ctx.Response.WriteAsync("HOME PAGE")));

//app.Run(async ctx =>
//{
//    ctx.Response.StatusCode = 404;
//    await ctx.Response.WriteAsync("Page not found :-(");
//});

//app.Run();





//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.Map("/api", app =>
//{
//    app.Map("/time", app => app.Run(async ctx => await ctx.Response.WriteAsync($"{DateTime.Now.ToShortTimeString()}")));
//    app.Map("/date", app => app.Run(async ctx => await ctx.Response.WriteAsync($"{DateTime.Now.ToShortDateString()}")));
//    app.Map("/home", app => app.Run(async ctx => await ctx.Response.WriteAsync("HOME PAGE")));

//    app.Run(async ctx =>
//    {
//        ctx.Response.StatusCode = 404;
//        await ctx.Response.WriteAsync("Endpoint not found :-(");
//    });
//});

//app.Run(async ctx =>
//{
//    ctx.Response.StatusCode = 404;
//    await ctx.Response.WriteAsync("Page not found :-(");
//});

//app.Run();






// ------------------- Пример с версиями API

//      /api/users
//      /api/v1/users
//      /api/v2/users


//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

////app.Use(...);
////app.Use(...);
////app.Use(...);

//app.Map("/api", apiApp =>
//{
//    apiApp.Use(async (ctx, next) =>
//    {
//        Console.WriteLine("API Logger\n");
//        await next();
//    });

//    apiApp.Map("/v1", v1App =>
//    {
//        v1App.Map("/users", v1AppUsers =>
//        {
//            v1AppUsers.Run(async ctx =>
//            {
//                await ctx.Response.WriteAsync("API V1 users\n");
//            });
//        });

//        v1App.Run(async ctx =>
//        {
//            ctx.Response.StatusCode = 404;
//            await ctx.Response.WriteAsync("API V1 Endpoint not found :-(");
//        });
//    });

//    apiApp.Map("/v2", v2App =>
//    {
//        v2App.Map("/users", v2AppUsers =>
//        {
//            v2AppUsers.Run(async ctx =>
//            {
//                await ctx.Response.WriteAsync("API V2 users\n");
//            });
//        });

//        v2App.Run(async ctx =>
//        {
//            ctx.Response.StatusCode = 404;
//            await ctx.Response.WriteAsync("API V2 Endpoint not found :-(");
//        });
//    });

//    apiApp.Run(async ctx =>
//    {
//        ctx.Response.StatusCode = 404;
//        await ctx.Response.WriteAsync("API Endpoint not found :-(");
//    });
//});

//app.Run(async ctx =>
//{
//    ctx.Response.StatusCode = 404;
//    await ctx.Response.WriteAsync("Page not found :-(");
//});

//app.Run();





// --------------------- Пример со статическими файлами

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();
//var env = app.Environment;

//app.Map("/static", staticApp =>
//{
//    staticApp.Run(async ctx =>
//    {
//        var filePath = Path.Combine(env.ContentRootPath, "static", Path.GetFileName(ctx.Request.Path));
//        ctx.Response.ContentType = "application/octet-stream";
//        await ctx.Response.SendFileAsync(filePath);
//    });
//});

//app.Run(async ctx =>
//{
//    ctx.Response.StatusCode = 404;
//    await ctx.Response.WriteAsync("Page not found :-(");
//});

//app.Run();



// --------------- Пример с аутентификацией

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();
//var env = app.Environment;

//// /api/public....
//// /api/secure....

//app.Map("/api/public", publicApp =>
//{
//    publicApp.Run(async ctx =>
//    {
//        await ctx.Response.WriteAsync("Public content");
//    });
//});

//app.Map("/api/secure", secureApp =>
//{
//    secureApp.Use(async (ctx, next) =>
//    {
//        if (ctx.Request.Headers["X-auth-token"] != "secret123")
//        {
//            ctx.Response.StatusCode = 401;
//            await ctx.Response.WriteAsync("User access denied");
//            return;
//        }

//        await next();
//    });

//    secureApp.Run(async ctx =>
//    {
//        await ctx.Response.WriteAsync("Secure content");
//    });
//});

//app.Run();


#endregion

#region Middleware classes

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.UseMiddleware<CustomMiddleware>();
//app.Run(async ctx => await ctx.Response.WriteAsync("Terminal MW"));
//app.Run();


//public class CustomMiddleware
//{
//    private readonly RequestDelegate next;

//    public CustomMiddleware(RequestDelegate next)
//    {
//        this.next = next;
//    }
//    public async Task InvokeAsync(HttpContext ctx)
//    {
//        Console.WriteLine("one");
//        await next(ctx);
//        Console.WriteLine("two");
//    }
//}





//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.UseCustomMiddleware();
//app.Run(async ctx => await ctx.Response.WriteAsync("Terminal MW"));
//app.Run();

//public class CustomMiddleware
//{
//    private readonly RequestDelegate next;

//    public CustomMiddleware(RequestDelegate next)
//    {
//        this.next = next;
//    }
//    public async Task InvokeAsync(HttpContext ctx)
//    {
//        Console.WriteLine("one");
//        await next(ctx);
//        Console.WriteLine("two");
//    }
//}

//public static class CustomAppExtensions
//{
//    public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder app)
//    {
//        return app.UseMiddleware<CustomMiddleware>();
//    }
//}





//using System.Diagnostics;

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.UseMiddleware<TimingMiddleware>(true);
//app.Run(async ctx => await ctx.Response.WriteAsync("Terminal MW"));
//app.Run();


//class TimingMiddleware
//{
//    private readonly RequestDelegate next;
//    private readonly bool logToConsole;

//    public TimingMiddleware(RequestDelegate next, bool logToConsole)
//    {
//        this.next = next;
//        this.logToConsole = logToConsole;
//    }

//    public async Task InvokeAsync(HttpContext ctx)
//    {
//        var sw = Stopwatch.StartNew();
//        await next(ctx);
//        sw.Stop();

//        if (logToConsole)
//            Console.WriteLine($"Request to {ctx.Request.Path} took {sw.ElapsedMilliseconds} ms");
//    }
//}






// ------------------ Пример с поддоменами

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseWhen(
    ctx => ctx.Request.Host.Host.StartsWith("admin."),
    branch => branch.UseMiddleware<AdminDashboardMiddleware>()
);

app.UseWhen(
    ctx => ctx.Request.Host.Host.StartsWith("api."),
    branch => branch.UseMiddleware<ApiVersioningMiddleware>()
);

app.MapGet("/", async ctx => 
{
    string data = $"dashboard: {ctx.Items["DashboardType"]}, api version: {ctx.Request.Headers["X-Api-Version"]}";
    await ctx.Response.WriteAsync(data);
});

app.Run();


public class AdminDashboardMiddleware
{
    private readonly RequestDelegate next;
    public AdminDashboardMiddleware(RequestDelegate next) => this.next = next;
    public async Task InvokeAsync(HttpContext ctx)
    {
        ctx.Items["DashboardType"] = "Admin";

        await next(ctx);
    }
}

public class ApiVersioningMiddleware
{
    private readonly RequestDelegate next;
    public ApiVersioningMiddleware(RequestDelegate next) => this.next = next;
    public async Task InvokeAsync(HttpContext ctx)
    {
        ctx.Request.Headers["X-Api-Version"] = "v2";

        await next(ctx);
    }
}


#endregion

