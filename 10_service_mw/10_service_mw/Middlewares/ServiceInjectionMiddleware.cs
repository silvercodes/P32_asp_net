using _10_service_mw.Services;

namespace _10_service_mw.Middlewares;

public class ServiceInjectionMiddleware
{
    private readonly RequestDelegate next;
    private readonly ITimeService timeService;

    public ServiceInjectionMiddleware(RequestDelegate next, ITimeService timeService)
    {
        this.next = next;
        this.timeService = timeService;
    }

    public async Task InvokeAsync(HttpContext ctx)
    {
        ctx.Response.Headers.Append("X-Server-Time", timeService.GetTime());

        await next(ctx);
    }
}
