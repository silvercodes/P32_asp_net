// Get products list            GET     /products
// Get specific product         GET     /products/{id}
// Create product               POST    /products
// Full update product          PUT     /products/{id}
// Update product               PATCH   /products/{id}
// Delete product               DELETE  /products/{id}


using System.Text.Json;
using _06_rest_api.Middlewares;
using _06_rest_api.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var env = app.Environment;

const string dataFileName = "products.json";
string dataFilePath = Path.Combine(env.ContentRootPath, "Data", dataFileName);
var jsonOptions = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
    WriteIndented = true
};

await InitDataAsync();

app.UseMiddleware<ProductsMiddleware>(jsonOptions, dataFilePath);
// app.UseMiddleware<NotFoundMiddleware>();

app.Run();

async Task InitDataAsync()
{
    if (!File.Exists(dataFilePath))
    {
        var initialProducts = new List<Product>
        {
            new Product { Id = 101, Name = "Laptop", Price = 1000.0m, Stock = 10, Category = "Electronics" },
            new Product { Id = 102, Name = "Smartphone", Price = 300.0m, Stock = 25, Category = "Electronics" }
        };

        await File.WriteAllTextAsync(dataFilePath, JsonSerializer.Serialize(initialProducts, jsonOptions));
    }
}
