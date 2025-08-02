namespace API.Endpoints.Tasks;

public class GetAllTasksEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/tasks", async (ITaskService _taskService) =>
        {
            IEnumerable<ToDoTaskResponse> tasks = await _taskService.GetAllTasks().ToListAsync();

            return Results.Ok(tasks);
        })
        .WithName("GetAllTasks")
        .WithTags("Tasks")
        .Produces<IEnumerable<ToDoTaskResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .WithOpenApi(opt =>
        {
            opt.Summary = "List all tasks";
            opt.Responses["200"] = new Microsoft.OpenApi.Models.OpenApiResponse { Description = "Successfull operation. Return the list of tasks, empty if there's no tasks" };
                        
            return opt;
        });       
    }
}