using System.Formats.Asn1;
using System.Text.Json;
using _06_rest_api.Models;

namespace _06_rest_api.Middlewares;

public class ProductsMiddleware
{
    private readonly RequestDelegate next;
    private readonly JsonSerializerOptions jsonOptions;
    private readonly string dataFilePath;

    public ProductsMiddleware(RequestDelegate next, JsonSerializerOptions jsonOptions, string dataFilePath)
    {
        this.next = next;
        this.jsonOptions = jsonOptions;
        this.dataFilePath = dataFilePath;
    }
    public async Task InvokeAsync(HttpContext ctx)
    {
        var req = ctx.Request;
        var res = ctx.Response;
        var path = req.Path;
        var method = req.Method;

        if (path.StartsWithSegments("/products"))
        {
            try
            {
                List<Product> products = await LoadProductsAsync();

                switch (method)
                {
                    case "GET" when path.Value == "/products":
                        await GetAllProductsAsync(res, products);
                        break;
                    case "GET" when path.Value?.StartsWith("/products") == true:
                        await GetProductByIdAsync(res, path, products);
                        break;
                    case "POST" when path.Value == "/products":
                        await CreateProductAsync(req, res, products);
                        break;
                    case "PUT" when path.Value?.StartsWith("/products") == true:
                        await UpdateProductAsync(req, res, path, products);
                        break;
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }

    private async Task<List<Product>> LoadProductsAsync()
    {
        await using var stream = File.OpenRead(dataFilePath);
        return await JsonSerializer.DeserializeAsync<List<Product>>(stream, jsonOptions)
            ?? new List<Product>();
    }
    private async Task SaveProductsAsync(List<Product> products)
    {
        await using var stream = File.Create(dataFilePath);
        await JsonSerializer.SerializeAsync(stream, products, jsonOptions);
    }

    private async Task GetAllProductsAsync(HttpResponse res, List<Product> products)
    {
        await res.WriteAsJsonAsync(products, jsonOptions);
    }
    private async Task GetProductByIdAsync(HttpResponse res, PathString path, List<Product> products)
    {
        int? id = ExtractIdFromPath(path);
        if (id.HasValue && products.FirstOrDefault(prod => prod.Id == id) is Product product)
        {
            await res.WriteAsJsonAsync(product, jsonOptions);
            return;
        }

        res.StatusCode = StatusCodes.Status404NotFound;
        await res.WriteAsync("Product not found");
    }
    private async Task CreateProductAsync(HttpRequest req, HttpResponse res, List<Product> products)
    {
        Product? p = await req.ReadFromJsonAsync<Product>(jsonOptions);

        if (p is null || string.IsNullOrWhiteSpace(p.Name) || p.Price <= 0)
        {
            res.StatusCode = StatusCodes.Status400BadRequest;
            await res.WriteAsync("Product data is invalid");
            return;
        }

        p.Id = products.Any() ? products.Max(prod => prod.Id) + 1 : 101;
        products.Add(p);
        await SaveProductsAsync(products);

        res.StatusCode = StatusCodes.Status201Created;
        await res.WriteAsJsonAsync(p, jsonOptions);

    }
    private async Task UpdateProductAsync(HttpRequest req, HttpResponse res, PathString path, List<Product> products)
    {
        int? id = ExtractIdFromPath(path);

        if (!id.HasValue)
        {
            res.StatusCode = StatusCodes.Status404NotFound;
            await res.WriteAsync("Product not found or invalid id");
            return;
        }

        var existingProduct = products.FirstOrDefault(prod => prod.Id == id.Value);
        if (existingProduct is null)
        {
            res.StatusCode = StatusCodes.Status404NotFound;
            await res.WriteAsync("Product not found");
            return;
        }

        var updatedData = await req.ReadFromJsonAsync<Product>(jsonOptions);

        if (updatedData is null || string.IsNullOrWhiteSpace(updatedData.Name) || updatedData.Price <= 0)
        {
            res.StatusCode = StatusCodes.Status400BadRequest;
            await res.WriteAsync("Product data is invalid");
            return;
        }

        existingProduct.Name = updatedData.Name;
        existingProduct.Price = updatedData.Price;
        existingProduct.Stock = updatedData.Stock;
        existingProduct.Category = updatedData.Category;

        await SaveProductsAsync(products);
        res.StatusCode = StatusCodes.Status204NoContent;
    }


    private int? ExtractIdFromPath(string path)
    {
        var segments = path.Split('/');
        if (segments.Length == 3 && int.TryParse(segments[2], out int id))
            return id;

        return null;
    }
}
