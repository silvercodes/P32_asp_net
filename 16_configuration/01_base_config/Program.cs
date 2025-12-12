//var builder = WebApplication.CreateBuilder(args);
//var config = builder.Configuration;


//var app = builder.Build();
//var conf = app.Configuration;


//app.MapGet("/", () => "Hello World!");

//app.Run();





//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//var config = app.Configuration;
//var maxItems = config["AppSettings:MaxItems"];
//var enableCache = config["AppSettings:EnableCache"];
//Console.WriteLine($"{maxItems} {enableCache}");

//app.MapGet("/", () => "Test string");

//app.Run();






//using Microsoft.Extensions.Options;

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

//var app = builder.Build();

//app.MapGet("/config", (IOptions<AppSettings> opt) =>
//{
//    return opt.Value;
//});


//app.Run();

//class AppSettings
//{
//    public int MaxItems { get; set; }
//    public bool EnableCache { get; set; }
//    public string Theme { get; set; } = "Light";
//}





//var builder = WebApplication.CreateBuilder(args);

//var app = builder.Build();

//app.MapGet("/config", (IConfiguration opt) => opt["AppSettings:MaxItems"]);

//app.Run();






using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("custom-settings.json", optional: true)
    .AddEnvironmentVariables("APP_")
    .AddInMemoryCollection(new Dictionary<string, string?>
    {
        ["Cache:Timeout"] = "30"
    });

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("Database"));
builder.Services.Configure<FeatureFlags>(
    builder.Configuration.GetSection("Features"));

var app = builder.Build();

app.MapGet("/config", (IConfiguration config) =>
{
    return new
    {
        Environment = config["ASPNETCORE_ENVIRONMENT"],
        LogLevel = config["Logging:LogLevel:Default"],
        CacheTimeout = config["Cache:Timeout"]
    };
});

app.MapGet("/db-settings", (IOptions<DatabaseSettings> options) => options.Value);

app.MapGet("/features", (IOptions<FeatureFlags> options) => options.Value);

app.Run();




class DatabaseSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public int Timeout { get; set; } = 30;
    public bool EnableLogging { get; set; }

}

class FeatureFlags
{
    public bool NewDashboard { get; set; }
    public bool ExperimentalFeatures { get; set; }
    public string Theme { get; set; } = "light";
}

