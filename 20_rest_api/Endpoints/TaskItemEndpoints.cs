using System.Security.Claims;
using rest_api.Models;
using rest_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace rest_api.Endpoints;

public static class TaskItemEndpoints
{
    public static void MapTaskItemEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/items")
            .RequireAuthorization();

        // Get all taskItems for authenticated user
        group.MapGet("/", async (
            ITaskItemService taskItemService,
            [FromServices] IHttpContextAccessor httpContextAccessor) =>
        {
            var userId = GetUserId(httpContextAccessor);
            return Results.Ok(await taskItemService.GetUserTaskItems(userId));
        });

        // Create a new TaskItem for authenticated user
        group.MapPost("/", async (
            [FromBody] TaskItem taskItem,
            ITaskItemService taskItemService,
            IHttpContextAccessor httpContextAccessor) =>
        {
            var userId = GetUserId(httpContextAccessor);
            var createdTaskItem = await taskItemService.CreateTaskItem(taskItem, userId);

            return Results.Created($"/items/{createdTaskItem.Id}", createdTaskItem);
        });

        // Get specific TaskItem by id
        group.MapGet("/{id}", async (
            int id,
            ITaskItemService taskItemService,
            IHttpContextAccessor httpContextAccessor) =>
        {
            var userId = GetUserId(httpContextAccessor);
            return Results.Ok(await taskItemService.GetTaskItemById(id, userId));
        });

        // Update TaskItem for authenticated user
        group.MapPut("/{id}", async (
            int id,
            [FromBody] TaskItem taskItem,
            ITaskItemService taskItemService,
            IHttpContextAccessor httpContextAccessor) => 
        {
            var userId = GetUserId(httpContextAccessor);
            taskItem.Id = id;
            await taskItemService.UpdateTaskItem(taskItem, userId);
            return Results.NoContent();
        });

        // Delete TaskItem for authenticated user
        group.MapDelete("/{id}", async (
            int id,
            ITaskItemService taskItemService,
            IHttpContextAccessor httpContextAccessor) =>
        {
            var userId = GetUserId(httpContextAccessor);
            await taskItemService.DeleteTaskItem(id, userId);
            return Results.NoContent();
        });
    }

    private static int GetUserId(IHttpContextAccessor httpContextAccessor)
    {
        var userIdFromClaim = httpContextAccessor.HttpContext?.User
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (int.TryParse(userIdFromClaim, out int userId))
        {
            return userId;
        }

        throw new UnauthorizedAccessException("Invalid user Id");
    }
}
