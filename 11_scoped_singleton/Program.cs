using scoped_singleton.Middlewares;
using scoped_singleton.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IDataProvider, DataProvider>();
builder.Services.AddSingleton<ICacheService, CacheService>();
builder.Services.AddSingleton<CacheUpdater>();


var app = builder.Build();

app.UseMiddleware<CacheMiddleware>();

app.MapGet("/cache", (ICacheService cache) =>
{
    return cache.GetCachData();
});

app.Run();
