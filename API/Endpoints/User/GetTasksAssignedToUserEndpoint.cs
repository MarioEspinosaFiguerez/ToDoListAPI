
namespace API.Endpoints.User;

public class GetTasksAssignedToUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        // Get all tasks assigned to user
        app.MapGet("/users/{id}/tasks", async (Guid id, [FromServices] IUserService _userService) =>
        {
            IQueryable<ToDoTaskResponse> tasks = await _userService.GetAllTasksAssignedToUser(id, null);

            return Results.Ok(await tasks.ToListAsync());
        })
        .WithName("GetTasksAssignedToUser")
        .WithTags("Users")
        .Produces<IEnumerable<ToDoTaskResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .WithOpenApi(opt =>
        {
            opt.Summary = "List all tasks assigned to User";
            opt.Responses["200"] = new Microsoft.OpenApi.Models.OpenApiResponse { Description = "Successfull operation. Return the list of tasks assigned to a user, empty if there's no tasks" };

            return opt;
        });

        // Get a Task assigned to a user
        app.MapGet("/users/{userId}/tasks/{taskId}", async (Guid taskId, Guid userId, [FromServices] IUserService _userService) =>
        {
            IQueryable<ToDoTaskResponse> tasks = await _userService.GetAllTasksAssignedToUser(userId, taskId);

            return Results.Ok(await tasks.ToListAsync());
        })
        .WithName("GetTaskAssignedToUser")
        .WithTags("Users")
        .Produces<IEnumerable<ToDoTaskResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .WithOpenApi(opt =>
        {
            opt.Summary = "List all tasks assigned to User";
            opt.Responses["200"] = new Microsoft.OpenApi.Models.OpenApiResponse { Description = "Successfull operation. Return the tasks assigned to a user, empty if there's no tasks" };

            return opt;
        });
    }
}
