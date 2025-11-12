namespace _01_intro;

public class LoggingMiddleware
{
    private readonly RequestDelegate next;
    public LoggingMiddleware(RequestDelegate next) => this.next = next;
    public async Task InvokeAsync(HttpContext ctx)
    {
        Console.WriteLine($"Request: {ctx.Request.Path}");
        await next(ctx);
        Console.WriteLine($"Response: {ctx.Response.StatusCode}");
    }
}
