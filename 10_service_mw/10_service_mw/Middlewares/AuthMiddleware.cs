using _10_service_mw.Services;

namespace _10_service_mw.Middlewares;

public class AuthMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<AuthMiddleware> logger;

    public AuthMiddleware(RequestDelegate next, ILogger<AuthMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }
    public async Task InvokeAsync(
        HttpContext ctx
    )
    {
        logger.LogWarning($"Auth check for request #{ctx.Items["ReqNumber"]}");

        if (! ctx.Request.Headers.ContainsKey("Authorization"))
        {
            ctx.Response.StatusCode = 401;
            await ctx.Response.WriteAsync("Unauthorized");
            return;
        }

        await next(ctx);
    }
}
