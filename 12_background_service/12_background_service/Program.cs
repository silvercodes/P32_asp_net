using _12_background_service.BackgroundServices;
using _12_background_service.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IDataProcessor, DataProcessor>();
builder.Services.AddSingleton<BackgroundWorker>();

var app = builder.Build();

// Start baackgroundService
app.Services.GetRequiredService<BackgroundWorker>();

app.MapGet("/", () => "Hello World!");

app.Run();
