var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("config.json", optional: true)
    .AddXmlFile("config.xml", optional: true)
    .AddIniFile("config.ini", optional: true);

var app = builder.Build();

app.MapGet("/", (IConfiguration config) =>
    $"AppName: {config["AppName"]}");

app.Run();
