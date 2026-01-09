using rest_api.Data;
using rest_api.Models;
using Microsoft.EntityFrameworkCore;

namespace rest_api.Services;

public class TaskItemService : ITaskItemService
{
    private readonly AppDbContext db;
    public TaskItemService(AppDbContext db)
    {
        this.db = db;
    }

    public async Task<IEnumerable<TaskItem>> GetUserTaskItems(int userId)
    {
        return await db.TaskItems
            .Where(t => t.UserId == userId)
            .ToListAsync();
    }

    public async Task<TaskItem> CreateTaskItem(TaskItem item, int userId)
    {
        item.UserId = userId;
        item.CreatedAt = DateTime.UtcNow;

        db.TaskItems.Add(item);
        await db.SaveChangesAsync();

        return item;
    }

    public async Task<TaskItem> GetTaskItemById(int id, int userId)
    {
        var taskItem = await db.TaskItems
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        return taskItem ?? throw new KeyNotFoundException("Task not found");
    }

    public async Task UpdateTaskItem(TaskItem item, int userId)
    {
        var existingTaskItem = await GetTaskItemById(item.Id, userId);

        existingTaskItem.Title = item.Title;
        existingTaskItem.Description = item.Description;
        existingTaskItem.IsCompleted = item.IsCompleted;
        existingTaskItem.CompletedAt = item.CompletedAt;

        await db.SaveChangesAsync();
    }
    public async Task DeleteTaskItem(int id, int userId)
    {
        var item = await GetTaskItemById(id, userId);
        db.TaskItems.Remove(item);
        await db.SaveChangesAsync();
    }
}
