using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppConfig>(builder.Configuration);
builder.Services.AddSingleton<ConfigService>();

var app = builder.Build();

app.MapGet("/config", (IOptions<AppConfig> options, ConfigService cs) => options.Value);

app.Run();



class ConfigService
{
    private readonly IConfiguration config;
    public ConfigService(IConfiguration config)
    {
        this.config = config;
        Console.WriteLine($"Database: {config["Database:ConnectionString"]}");
    }
}

class DatabaseConfig
{
    public string ConnectionString { get; set; } = "default";
    public int MaxConnections { get; set; } = 100;
}

class AppConfig
{
    public string Environment { get; set; } = "Development";
    public int Timeout { get; set; } = 30;
    public DatabaseConfig Database { get; set; } = new();
}
