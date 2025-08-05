namespace API.Endpoints.Tasks
{
    public class DeleteTaskEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("/tasks/{id}", async (Guid id, [FromServices] ITaskService _taskService) => Results.Ok(await _taskService.DeleteTask(id)))
            .WithName("DeleteTask")
            .WithTags("Tasks")
            .Produces<bool>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithOpenApi(opt =>
            {
                opt.Summary = "Delete a task by their ID";
                opt.Responses["200"] = new Microsoft.OpenApi.Models.OpenApiResponse { Description = "Successfull operation. Task deleted" };

                return opt;
            }); ;
        }
    }
}
