
namespace API.Endpoints.Tasks;

public class GetAllTasksEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        // Assign Multiples routes using the same Handler
        async Task<IResult> GetAllTasks(ITaskService _taskService)
        {
            var tasks = await _taskService.GetAllTasks();
            return Results.Ok(tasks);
        }

        app.MapGet("/tasks", GetAllTasks).WithTags("Tasks");
        app.MapGet("/tasks/getAllTasks", GetAllTasks).WithTags("Tasks");

        /* Using 1 Route for 1 Handler
        app.MapGet("/tasks", async (ITaskService _taskService) =>
        {
            var tasks = await _taskService.GetAllTasks();
            return Results.Ok(tasks);
        });
        */
    }
}