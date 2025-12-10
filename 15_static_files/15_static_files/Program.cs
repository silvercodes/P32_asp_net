using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// -----------------------
// GET /    --> index.html  -->  index.htm  -->  default.html  -->  default.html
//app.UseDefaultFiles();
//app.UseStaticFiles();
//app.UseRouting();
//app.UseEndpoints(eps => { });

//app.MapGet("/", () => "Test string");

// -----------------------
//app.UseStaticFiles(new StaticFileOptions
//{
//    ServeUnknownFileTypes = true,
//    DefaultContentType = "application/octet-stream",
//    OnPrepareResponse = staticCtx =>
//    {
//        staticCtx.Context.Response.Headers.Append("X-Test", "Vasia");
//    }
//});

//app.MapGet("/", () => "Test string");

// -------------------------
//app.UseStaticFiles();
//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(
//        Path.Combine(builder.Environment.ContentRootPath, "CustomFiles")),
//    RequestPath = "/custom"
//});


// --------------------------
app.MapStaticAssets();

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = staticCtx =>
    {
        staticCtx.Context.Response.Headers.Append("Cache-Control", "public,max-age=3600");
    }
});

app.MapFallbackToFile("index.html");


app.Run();
