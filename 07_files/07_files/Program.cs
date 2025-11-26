using System.IO.Compression;
using System.Text.Json;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var env = app.Environment;

// === Для больших файлов ===
//app.Run(async ctx =>
//{
//    var req = ctx.Request;
//    var res = ctx.Response;

//    if (req.Path == "/download/bigfile")
//    {
//        var filePath = Path.Combine(env.ContentRootPath, "Storage", "patterns.pdf");
//        if (File.Exists(filePath))
//        {
//            res.ContentType = "application/pdf";
//            await res.SendFileAsync(filePath);
//        }
//        else
//            res.StatusCode = 404;
//    }
//});


//app.Run(async ctx =>
//{
//    var req = ctx.Request;
//    var res = ctx.Response;

//    if (req.Path == "/download/stream")
//    {
//        var filePath = Path.Combine(env.ContentRootPath, "Storage", "patterns.pdf");
//        if (File.Exists(filePath))
//        {
//            await using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

//            res.ContentType = "application/pdf";
//            res.Headers.ContentDisposition = $"attachment; filename={Path.GetFileName(filePath)}";

//            await fs.CopyToAsync(res.Body);
//        }
//        else
//            res.StatusCode = 404;
//    }
//});



// === Динамическая генерация файла
//app.Run(async ctx =>
//{
//    var req = ctx.Request;
//    var res = ctx.Response;

//    if (req.Path == "/download/generate")
//    {
//        res.ContentType = "text/plain";
//        res.Headers.ContentDisposition = $"attachment; filename=dynamic.txt";

//        for (int i = 0; i < 1000; ++i)
//        {
//            await Task.Delay(10);
//            await res.WriteAsync($"Line {i}\n");
//            await res.Body.FlushAsync();
//        }
//    }
//});


// === Отправка с архивацией на лету
//app.Run(async ctx =>
//{
//    var req = ctx.Request;
//    var res = ctx.Response;

//    if (req.Path == "/download/zip")
//    {
//        res.ContentType = "application/zip";
//        res.Headers.ContentDisposition = $"attachment; filename=archive.zip";

//        using var zipArchive = new ZipArchive(res.BodyWriter.AsStream(), ZipArchiveMode.Create);
//        var entry = zipArchive.CreateEntry("data.txt");

//        await using var writer = new StreamWriter(entry.Open());
//        await writer.WriteLineAsync("Data from server");
//    }
//});



app.Run(async ctx =>
{
    var req = ctx.Request;
    var res = ctx.Response;

    if (req.Path == "/download/multizip")
    {
        res.ContentType = "application/zip";
        res.Headers.ContentDisposition = $"attachment; filename=archive.zip";

        using var zipArchive = new ZipArchive(res.BodyWriter.AsStream(), ZipArchiveMode.Create);

        var textEntry = zipArchive.CreateEntry("note.txt");
        await using (var writer = new StreamWriter(textEntry.Open()))
        {
            await writer.WriteLineAsync("Data from server");
            await writer.WriteLineAsync($"Current time: {DateTime.Now.ToShortTimeString()}");
        }

        var csvEntry = zipArchive.CreateEntry("data.csv");
        await using (var csvWriter = new StreamWriter(csvEntry.Open()))
        {
            await csvWriter.WriteLineAsync("Id;Name;Value");
            for (int i = 0; i < 10; i++)
                await csvWriter.WriteLineAsync($"{i};item_{i};{i * 100}");
        }

        var jsonEntry = zipArchive.CreateEntry("data.json");
        await using (var jsonWriter = new StreamWriter(jsonEntry.Open()))
        {
            await jsonWriter.WriteLineAsync(
                    JsonSerializer.Serialize(new
                    {
                        Date = DateTime.Now.ToShortDateString(),
                        Value = 456,
                        Comment = "Hello"
                    })
                );
        }
    }
});


app.Run();
