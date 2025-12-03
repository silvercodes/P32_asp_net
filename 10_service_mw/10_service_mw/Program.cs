using _10_service_mw.Middlewares;
using _10_service_mw.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ITimeService, TimeService>();
builder.Services.AddSingleton<IRequestCounter, RequestCounter>();
builder.Services.AddLogging(builder =>
{
    builder
        .AddConsole()
        .SetMinimumLevel(LogLevel.Debug);
});

var app = builder.Build();

app.UseMiddleware<ServiceInjectionMiddleware>();
app.UseMiddleware<ScopedServiceMiddleware>();
app.UseMiddleware<AuthMiddleware>();

app.MapGet("/", (HttpContext ctx) =>
{
    int reqNumber = (int)ctx.Items["ReqNumber"];
    return $"Request: #{reqNumber} at {ctx.Response.Headers["X-Server-Time"]}";
});

app.MapGet("/secure", (HttpContext ctx) =>
{
    int reqNumber = (int)ctx.Items["ReqNumber"];
    return $"SECURE Request: #{reqNumber}";
});

app.Run();
