using rest_api.Models;

namespace rest_api.Services;

public interface ITaskItemService
{
    Task<IEnumerable<TaskItem>> GetUserTaskItems(int userId);
    Task<TaskItem> CreateTaskItem(TaskItem item, int userId);
    Task<TaskItem> GetTaskItemById(int id, int userId);
    Task UpdateTaskItem(TaskItem item, int userId);
    Task DeleteTaskItem(int id, int userId);
}
