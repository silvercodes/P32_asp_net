using _10_service_mw.Services;

namespace _10_service_mw.Middlewares;

public class ScopedServiceMiddleware
{
    private readonly RequestDelegate next;
    private readonly IRequestCounter counter;

    public ScopedServiceMiddleware(RequestDelegate next, IRequestCounter counter)
    {
        this.next = next;
        this.counter = counter;
    }

    public async Task InvokeAsync(HttpContext ctx)
    {
        var reqNumber = counter.Increment();
        ctx.Items["ReqNumber"] = reqNumber;
        await next(ctx);
    }
}
