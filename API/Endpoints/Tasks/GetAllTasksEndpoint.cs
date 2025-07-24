namespace API.Endpoints.Tasks;

public class GetAllTasksEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/tasks", async (ITaskService _taskService) =>
        {
            var tasks = await _taskService.GetAllTasks();
            return Results.Ok(tasks);
        })
        .WithName("GetAllTasks")
        .WithTags("Tasks")
        .Produces<IEnumerable<ToDoTaskDTO>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .WithOpenApi(opt =>
        {
            opt.Summary = "List all tasks";
            opt.Responses = new Microsoft.OpenApi.Models.OpenApiResponses
            {
                ["200"] = new Microsoft.OpenApi.Models.OpenApiResponse { Description = "Sucessfull operation. Return the list of tasks, empty if there's no tasks" },
                ["500"] = new Microsoft.OpenApi.Models.OpenApiResponse { Description = "Internal server error" }
            };

            return opt;
        });       
    }
}