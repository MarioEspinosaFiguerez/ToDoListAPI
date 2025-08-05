
namespace API.Endpoints.Tasks;

public class UpdateTaskEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("/tasks/{id}", async (Guid id, UpdateToDoTaskRequest request, [FromServices]ITaskService _taskService, [FromServices]IValidator<UpdateToDoTaskRequest> validator) =>
        {
            ValidationResult result = await validator.ValidateAsync(request);
            if (!result.IsValid) { return Validation.GetValidationProblemErrors(result); }

            ToDoTaskResponse updatedTask = await _taskService.UpdateTask(id, request);
            return Results.Ok(updatedTask);
        })
        .WithName("UpdateTask")
        .WithTags("Tasks")
        .ProducesValidationProblem(StatusCodes.Status400BadRequest)
        .Produces<UserDTO>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .WithOpenApi(opt =>
        {
            opt.Summary = "Update a task";
            opt.Responses["200"] = new Microsoft.OpenApi.Models.OpenApiResponse { Description = "Successfull operation. Task updated" };
            opt.Responses["404"] = new Microsoft.OpenApi.Models.OpenApiResponse { Description = "Error in operation. Task not found" };
            return opt;
        }); ;
    }
}
