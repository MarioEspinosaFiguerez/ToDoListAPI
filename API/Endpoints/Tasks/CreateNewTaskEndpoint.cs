namespace API.Endpoints.Tasks;

public class CreateNewTaskEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/tasks", async ([FromServices]ITaskService _taskService, [FromServices]IValidator<CreateToDoTaskRequest> validator, CreateToDoTaskRequest request) =>
        {
            ValidationResult result = await validator.ValidateAsync(request);

            if (!result.IsValid) { return Validation.GetValidationProblemErrors(result); }

            ToDoTaskResponse createdTask = await _taskService.CreateTask(request);

            return Results.Created($"/tasks/{createdTask.Id}", createdTask);
        })
        .WithName("CreateTask")
        .WithTags("Tasks")
        .ProducesValidationProblem(StatusCodes.Status400BadRequest)
        .Produces<ToDoTask>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .WithOpenApi(opt =>
        {
            opt.Summary = "Create new Task";
            opt.Responses["201"] = new Microsoft.OpenApi.Models.OpenApiResponse { Description = "Successfull operation, task created correctly" };

            return opt;
        });
    }
}