namespace API.Endpoints.Tasks;

public class GetTaskByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/tasks/{id}", async (ITaskService _taskService, Guid id) =>
        {
            ToDoTaskResponse taskById = await _taskService.GetTaskById(id);

            return Results.Ok(taskById);
        })
        .WithName("GetTaskById")
        .WithTags("Tasks")
        .Produces<ToDoTaskResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .WithOpenApi(opt =>
        {
            opt.Summary = "Get a task by its ID";
            opt.Responses["200"] = new Microsoft.OpenApi.Models.OpenApiResponse { Description = "Successfull operation. Return the Task by Id" };
            return opt;
        });
    }
}
