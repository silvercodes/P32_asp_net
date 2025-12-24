using System.Reflection.Metadata.Ecma335;
using _18_db_using.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// dotnet tool install --global dotnet-ef
// dotnet ef migrations add Initial
// dotnet ef database update

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Db>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Db>();
    db.Database.Migrate();
}

app.MapPost("/tasks", async (TaskItem item, Db db) =>
{
    db.TaskItems.Add(item);
    await db.SaveChangesAsync();

    return Results.Created($"/tasks/{item.Id}", item);
});

app.MapGet("/tasks", async (Db db) =>
    await db.TaskItems.ToListAsync());

app.MapGet("/tasks/{id:int}", async (int id, Db db) =>
    await db.TaskItems.FindAsync(id) is TaskItem ti ?
        Results.Ok(ti) :
        Results.NotFound());

app.MapPut("/tasks/{id:int}", async (int id, [FromBody] TaskItem inputItem, Db db) => 
{
    var item = await db.TaskItems.FindAsync(id);
    if (item is null)
        return Results.NotFound();

    item.Title = inputItem.Title;
    item.IsCompleted = inputItem.IsCompleted;

    if (item.IsCompleted && item.CompletedAt is null)
        item.CompletedAt = DateTime.UtcNow;
    else if (!item.IsCompleted)
        item.CompletedAt = null;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapGet("/tasks/completed", async (Db db) =>
    await db.TaskItems
        .Where(t => t.IsCompleted)
        .ToListAsync());

app.MapGet("/tasks/search/{term}", async(string term, Db db) =>
    await db.TaskItems
        .Where(t => t.Title.Contains(term))
        .ToListAsync());

app.Run();
