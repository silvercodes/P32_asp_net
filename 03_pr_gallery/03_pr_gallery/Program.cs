// WebRootPath - путь к директории с публичными файлами (по-умолчанию wwwroot)
// ContentRootPath - путь к корню проекта


using System.Text;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
{
    WebRootPath = "public"
});

var app = builder.Build();

app.UseStaticFiles();

app.Use(async (ctx, next) =>
{
    HttpRequest req = ctx.Request;

    if (req.Path == "/" && req.Method == "GET")
    {
        var templatePath = Path.Combine(app.Environment.ContentRootPath, "Templates", "index.html");
        var htmlContent = await File.ReadAllTextAsync(templatePath);

        var imageDirectory = Path.Combine(app.Environment.WebRootPath, "storage");
        var imageFiles = Directory.GetFiles(imageDirectory);

        var imgHtml = new StringBuilder();
        foreach(var item in imageFiles)
        {
            var fileName = Path.GetFileName(item);
            imgHtml.AppendLine(
                $@" <a href=""/download/{fileName}"">
                        <img src=""/storage/{fileName}"" width=""200""/>
                    </a>"    
            );
        }

        var finalHtml = htmlContent.Replace("<!--IMAGES-->", imgHtml.ToString());
        ctx.Response.ContentType = "text/html; charset=utf-8";
        await ctx.Response.WriteAsync(finalHtml);
    }
    else
    {
        await next(ctx);
    }
});

app.Use(async (ctx, next) =>
{
    HttpRequest req = ctx.Request;
    HttpResponse res = ctx.Response;

    if (req.Method == "GET" && req.Path.StartsWithSegments("/download", out var remaining))
    {
        var imageName = Path.GetFileName(remaining.ToString());

        if (string.IsNullOrEmpty(imageName))
        {
            res.StatusCode = StatusCodes.Status400BadRequest;
            await res.WriteAsync("Inavalid image name");
            return;
        }

        var imageDirectory = Path.Combine(app.Environment.WebRootPath, "storage");
        var imagePath = Path.Combine(imageDirectory, imageName);

        if (! File.Exists(imagePath))
        {
            res.StatusCode = StatusCodes.Status404NotFound;
            await res.WriteAsync("Image not found");
            return;
        }

        // --- download file
        // res.ContentType = "application/octet-stream";
        // res.Headers["Content-Disposition"] = $"attachment; filename={imageName}";

        // --- open file
        res.ContentType = "image/jpg";

        await using (var fileStream = File.OpenRead(imagePath))
        {
            await fileStream.CopyToAsync(res.Body);
        }
    }
    else
    {
        await next(ctx);
    }
});



app.Run();
