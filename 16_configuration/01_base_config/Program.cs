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





var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/config", (IConfiguration opt) => opt["AppSettings:MaxItems"]);

app.Run();